using BoilerPlate.Core.Exceptions.Enums;

namespace BoilerPlate.Core.Exceptions.Exceptions;

/// <summary>
/// Conflict error exception
/// </summary>
/// <errorCode> 409 </errorCode>
public class ConflictException(ExceptionCode code, string message) : BaseExceptionWithCode(code, message);