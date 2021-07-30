using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlitRenderPassFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {

        public RenderTargetIdentifier source;

        private Material material;
        private int materialPassIndex;
        private string name;
        private RenderTargetHandle tempRenderTargetHandler;

        public CustomRenderPass(Material material, int passIndex, string profName)
        {
            this.material = material;
            this.materialPassIndex = passIndex;
            this.name = profName;
            tempRenderTargetHandler.Init("_TemporaryColorTexture");
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get(name);
            commandBuffer.GetTemporaryRT(tempRenderTargetHandler.id, renderingData.cameraData.cameraTargetDescriptor);

            Blit(commandBuffer, source, tempRenderTargetHandler.Identifier(), material, materialPassIndex);
            Blit(commandBuffer, tempRenderTargetHandler.Identifier(), source);

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempRenderTargetHandler.id);
        }
    }

    CustomRenderPass m_ScriptablePass;

    [System.Serializable]
    public class Settings
    {
        public Material material;
        public string profilingName;
        public int materialPassIndex = -1;
    }

    [SerializeField]
    public Settings settings = new Settings();

    public Material Material
    {
        get => settings.material;
    }

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(settings.material, settings.materialPassIndex, settings.profilingName);
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.source = renderer.cameraColorTarget;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


