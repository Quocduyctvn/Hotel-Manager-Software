using Hotel.Share.Const;
using System.Security.Claims;
namespace Hotel.Share.Extension
{
	public static class AppExtension
	{

		public static bool IsInPermission(this ClaimsPrincipal user, int actionPermission)
		{
			// Find the user’s permissions claim
			var userPermission = user?.FindFirst(AppClaimTypes.Permissions);

			// Return false if the permission claim is not present or its value is empty
			if (userPermission == null || string.IsNullOrEmpty(userPermission.Value))
			{
				return false;
			}

			// If the user has no permissions (special case), always return true
			if (actionPermission == AuthConst.NO_PERMISSION)
			{
				return true;
			}

			// Check if the user’s permissions contain the specified actionPermission (in string format)
			return userPermission.Value.Split(',').Any(permission => permission.Trim() == actionPermission.ToString());
		}

	}
}
