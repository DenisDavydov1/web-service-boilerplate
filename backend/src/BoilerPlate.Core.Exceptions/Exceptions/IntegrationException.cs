using BoilerPlate.Core.Exceptions.Enums;

namespace BoilerPlate.Core.Exceptions.Exceptions;

/// <summary>
/// External integration error exception
/// </summary>
/// <errorCode> 500 </errorCode>
public class IntegrationException(ExceptionCode code, string message) : BaseExceptionWithCode(code, message);