// Crest Ocean System

// This file is subject to the MIT License as seen in the root of this folder structure (LICENSE)

using UnityEngine;
using UnityEngine.Rendering;

namespace Crest
{
    /// <summary>
    /// Base class for the command buffer builder, which takes care of updating all ocean-related data. If you wish to provide your
    /// own update logic, you can create a new component that inherits from this class and attach it to the same GameObject as the
    /// OceanRenderer script. The new component should be set to update after the Default bucket, similar to BuildCommandBuffer.
    /// </summary>
    public abstract class BuildCommandBufferBase : MonoBehaviour
    {
        /// <summary>
        /// Used to validate update order
        /// </summary>
        public static int _lastUpdateFrame = -1;
    }

    /// <summary>
    /// The default builder for the ocean update command buffer which takes care of updating all ocean-related data, for
    /// example rendering animated waves and advancing sims. This runs in LateUpdate after the Default bucket, after the ocean
    /// system been moved to an up to date position and frame processing is done.
    /// </summary>
    public class BuildCommandBuffer : BuildCommandBufferBase
    {
        // 変更
        OceanRenderer[] oceans = new OceanRenderer[2];

//        CommandBuffer[] _buf = new CommandBuffer[2];
        CommandBuffer _buf;


        //CommandBuffer _buf;

        void Build(OceanRenderer ocean, CommandBuffer buf)
        {

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // --- Ocean depths
            if (ocean._lodDataSeaDepths)
            {
                ocean._lodDataSeaDepths.BuildCommandBuffer(ocean, buf);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // --- Flow data
            if (ocean._lodDataFlow)
            {
                ocean._lodDataFlow.BuildCommandBuffer(ocean, buf);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // --- Dynamic wave simulations
            if (ocean._lodDataDynWaves)
            {
                ocean._lodDataDynWaves.BuildCommandBuffer(ocean, buf);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // --- Animated waves next
            if (ocean._lodDataAnimWaves)
            {
                ocean._lodDataAnimWaves.BuildCommandBuffer(ocean, buf);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // --- Foam simulation
            if (ocean._lodDataFoam)
            {
                ocean._lodDataFoam.BuildCommandBuffer(ocean, buf);
            }
        }

        /// <summary>
        /// Construct the command buffer and attach it to the camera so that it will be executed in the render.
        /// </summary>
        public void LateUpdate()
        {
            /*
            if (false)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (oceans[i] == null) return;

                    if (_buf[i] == null)
                    {
                        _buf[i] = new CommandBuffer();
                        _buf[i].name = "CrestLodData";
                    }
                    _buf[i].Clear();

                    // あやしい
                    //           Build(OceanRenderer.Instance, _buf);

                    Build(oceans[i], _buf[i]);


                    // This will execute at the beginning of the frame before the graphics queue
                    Graphics.ExecuteCommandBuffer(_buf[i]);
                }
            }

            else
            */
            {
                if (OceanRenderer.Instance == null) return;
                if (_buf == null)
                {
                    _buf = new CommandBuffer();
                    _buf.name = "CrestLodData";
                }
                _buf.Clear();

                // あやしい
                 Build(OceanRenderer.Instance, _buf);

                Graphics.ExecuteCommandBuffer(_buf);


            }


            _lastUpdateFrame = Time.frameCount;
        }

        // 変更
        public void SetOcean(int num, OceanRenderer ocean)
        {
            // 最初
            Debug.Log("最初");
            oceans[num] = ocean;
            Debug.Log("num:" + num);
        }
    }
   
}
