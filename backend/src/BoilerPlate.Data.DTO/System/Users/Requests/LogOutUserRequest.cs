using MediatR;

namespace BoilerPlate.Data.DTO.System.Users.Requests;

/// <summary>
/// Log out user (delete token info)
/// </summary>
public class LogOutUserRequest : IRequest;