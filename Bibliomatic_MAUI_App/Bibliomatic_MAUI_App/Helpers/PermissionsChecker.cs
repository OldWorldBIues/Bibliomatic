namespace Bibliomatic_MAUI_App.Helpers
{
    public class PermissionsChecker
    {
        public static async Task RequestRequiredPermissions()
        {            
            await Permissions.RequestAsync<Permissions.Photos>().WaitAsync(CancellationToken.None);
            await Permissions.RequestAsync<Permissions.Media>().WaitAsync(CancellationToken.None);
            await Permissions.RequestAsync<Permissions.Camera>().WaitAsync(CancellationToken.None);

            if (DeviceInfo.Current.Platform == DevicePlatform.Android && Convert.ToInt32(DeviceInfo.Current.VersionString) < 13)
            {
                await Permissions.RequestAsync<Permissions.StorageRead>().WaitAsync(CancellationToken.None);
                await Permissions.RequestAsync<Permissions.StorageWrite>().WaitAsync(CancellationToken.None);
            }
        }

        private static async Task<bool> AllRequiredPermissionsGranted()
        {
            var mediaPermission = await Permissions.CheckStatusAsync<Permissions.Media>().WaitAsync(CancellationToken.None);
            var photosPermission = await Permissions.CheckStatusAsync<Permissions.Photos>().WaitAsync(CancellationToken.None);
            var cameraPermission = await Permissions.CheckStatusAsync<Permissions.Camera>().WaitAsync(CancellationToken.None);

            var permissionsList = new List<PermissionStatus> { mediaPermission, photosPermission, cameraPermission };

            if (DeviceInfo.Current.Platform == DevicePlatform.Android & Convert.ToInt32(DeviceInfo.Current.VersionString) < 13)
            {
                var storageReadPermission = await Permissions.CheckStatusAsync<Permissions.StorageRead>().WaitAsync(CancellationToken.None);
                var storageWritePermission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>().WaitAsync(CancellationToken.None);

                permissionsList.Add(storageReadPermission);
                permissionsList.Add(storageWritePermission);
            }

            return permissionsList.All(permission => permission == PermissionStatus.Granted);
        }

        public static async Task<bool> CheckForRequiredPermissions()
        {
            bool permissionsGranted = await AllRequiredPermissionsGranted();

            if(!permissionsGranted)
            {
                var action = await Application.Current.MainPage.DisplayAlert("Permissions not granted", "Do you want to try to re-grant the required permissions?", "Yes", "No");

                if(action)
                {
                    await RequestRequiredPermissions();
                    permissionsGranted = await AllRequiredPermissionsGranted();

                    if (permissionsGranted) return true;
                }

                await Application.Current.MainPage.DisplayAlert("Required permissions not granted", "You need to grant required to pick files from your device. Please grant all required permissions and try again", "Got it");
                return false;
            }

            return true;
        }
    }
}
