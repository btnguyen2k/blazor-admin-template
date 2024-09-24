﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bat.Shared.Identity;

public class UserFetchOptions
{
	public static readonly UserFetchOptions DEFAULT = new();

	public bool IncludeRoles { get; set; } = false;
	public bool IncludeClaims { get; set; } = false;
	public bool IncludeRoleClaims { get; set; } = false;
}

public class RoleFetchOptions
{
	public static readonly RoleFetchOptions DEFAULT = new();

	public bool IncludeClaims { get; set; } = false;
}

public interface IIdentityRepository
{
	/// <summary>
	/// Creates a new user.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> CreateAsync(BatUser user, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new user if it does not exist.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> CreateIfNotExistsAsync(BatUser user, CancellationToken cancellationToken = default);

	ValueTask<BatUser?> GetUserByIDAsync(string userId, UserFetchOptions? options = default, CancellationToken cancellationToken = default);
	ValueTask<BatUser?> GetUserByEmailAsync(string email, UserFetchOptions? options = default, CancellationToken cancellationToken = default);
	ValueTask<BatUser?> GetUserByUserNameAsync(string userName, UserFetchOptions? options = default, CancellationToken cancellationToken = default);
	IAsyncEnumerable<BatUser> AllUsersAsync();

	/// <summary>
	/// Updates the user.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>The user with updated data.</returns>
	/// <remarks>
	///		null is returned if the update operated didnot succeed.
	///		user's concurrency stamp is automatically updated and reflected in the returned instance.
	///	</remarks>
	ValueTask<BatUser?> UpdateAsync(BatUser user, CancellationToken cancellationToken = default);

	///// <summary>
	///// Updates the security stamp of the user.
	///// </summary>
	///// <param name="user"></param>
	///// <param name="cancellationToken"></param>
	///// <returns>The user with new security stamp</returns>
	///// <remarks>
	/////		null is returned if the update operated didnot succeed.
	/////		user's concurrency stamp is automatically updated and reflected in the returned instance.
	/////	</remarks>
	//ValueTask<BatUser?> UpdateSecurityStampAsync(BatUser user, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an existing user.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	///		If the user does not exist, the operation is considered successful.
	/// </remarks>
	ValueTask<IdentityResult> DeleteAsync(BatUser user, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves the roles of the user.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="roleFetchOptions"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	///		If the user has no roles, an empty collection is returned.
	///		If the user does not exist, null is returned.
	/// </remarks>
	ValueTask<IEnumerable<BatRole>> GetRolesAsync(BatUser user, RoleFetchOptions? roleFetchOptions = default, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves the claims of the user.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	///		If the user has no claims, an empty collection is returned.
	///		If the user does not exist, null is returned.
	/// </remarks>
	ValueTask<IEnumerable<IdentityUserClaim<string>>> GetClaimsAsync(BatUser user, CancellationToken cancellationToken = default);

	//ValueTask<bool> HasRoleAsync(BatUser user, BatRole role, CancellationToken cancellationToken = default);
	//ValueTask<bool> HasRoleAsync(BatUser user, string roleName, CancellationToken cancellationToken = default);

	ValueTask<IdentityResult> AddToRolesAsync(BatUser user, IEnumerable<BatRole> roles, CancellationToken cancellationToken = default);
	ValueTask<IdentityResult> AddToRolesAsync(BatUser user, IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
	ValueTask<IdentityResult> AddToRoleIfNotExistsAsync(BatUser user, BatRole role, CancellationToken cancellationToken = default);
	ValueTask<IdentityResult> AddToRoleIfNotExistsAsync(BatUser user, string roleName, CancellationToken cancellationToken = default);

	/// <summary>
	/// Removes the user from the specified roles.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="roles"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	///		If the user is not in the specified roles, the operation is considered successful.
	/// </remarks>
	ValueTask<IdentityResult> RemoveFromRolesAsync(BatUser user, IEnumerable<BatRole> roles, CancellationToken cancellationToken = default);

	/// <summary>
	/// Removes the user from the specified roles.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="roleNames"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	///		If the user is not in the specified roles, the operation is considered successful.
	/// </remarks>
	ValueTask<IdentityResult> RemoveFromRolesAsync(BatUser user, IEnumerable<string> roleNames, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds claims to the user.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="claims"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> AddClaimsAsync(BatUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds a claim to the user, if it does not exist.
	/// </summary>
	/// <param name="user"></param>
	/// <param name="claim"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> AddClaimIfNotExistsAsync(BatUser user, Claim claim, CancellationToken cancellationToken = default);

	/*----------------------------------------------------------------------*/

	/// <summary>
	/// Creates a new role.
	/// </summary>
	/// <param name="role"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> CreateAsync(BatRole role, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new role if it does not exist.
	/// </summary>
	/// <param name="role"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> CreateIfNotExistsAsync(BatRole role, CancellationToken cancellationToken = default);

	ValueTask<BatRole?> GetRoleByIDAsync(string roleId, RoleFetchOptions? options = default, CancellationToken cancellationToken = default);
	ValueTask<BatRole?> GetRoleByNameAsync(string roleName, RoleFetchOptions? options = default, CancellationToken cancellationToken = default);

	IAsyncEnumerable<BatRole> AllRolesAsync();

	/// <summary>
	/// Updates the role.
	/// </summary>
	/// <param name="role"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>The role with updated data.</returns>
	/// <remarks>
	///		null is returned if the update operated didnot succeed.
	///		role's concurrency stamp is automatically updated and reflected in the returned instance.
	///	</remarks>
	ValueTask<BatRole?> UpdateAsync(BatRole role, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes an existing role.
	/// </summary>
	/// <param name="role"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	///		If the role does not exist, the operation is considered successful.
	/// </remarks>
	ValueTask<IdentityResult> DeleteAsync(BatRole role, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves the claims of the role.
	/// </summary>
	/// <param name="role"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <remarks>
	///		If the role has no claims, an empty collection is returned.
	///		If the role does not exist, null is returned.
	/// </remarks>
	ValueTask<IEnumerable<IdentityRoleClaim<string>>> GetClaimsAsync(BatRole role, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds claims to the role.
	/// </summary>
	/// <param name="role"></param>
	/// <param name="claims"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> AddClaimsAsync(BatRole role, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds a claim to the role, if it does not exist.
	/// </summary>
	/// <param name="role"></param>
	/// <param name="claim"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	ValueTask<IdentityResult> AddClaimIfNotExistsAsync(BatRole role, Claim claim, CancellationToken cancellationToken = default);
}
