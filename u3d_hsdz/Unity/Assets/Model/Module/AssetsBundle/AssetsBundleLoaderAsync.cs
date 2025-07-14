using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class AssetsBundleLoaderAsyncSystem : UpdateSystem<AssetsBundleLoaderAsync>
    {
        public override void Update(AssetsBundleLoaderAsync self)
        {
            self.Update();
        }
    }

    public class AssetsBundleLoaderAsync : Component
    {
        private AssetBundleCreateRequest request;

        private TaskCompletionSource<AssetBundle> tcs;

        public void Update()
        {
            if (!request.isDone)
            {
                return;
            }

            TaskCompletionSource<AssetBundle> t = tcs;
            t.SetResult(request.assetBundle);
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            base.Dispose();
        }

        public Task<AssetBundle> LoadAsync(string path, int encrypt)
        {
            tcs = new TaskCompletionSource<AssetBundle>();
            request = encrypt == 1? AssetBundle.LoadFromFileAsync(path, 0, 128) : AssetBundle.LoadFromFileAsync(path);
            return tcs.Task;
        }

    }
}
