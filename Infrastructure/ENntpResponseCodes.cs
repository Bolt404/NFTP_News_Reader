namespace NNTP_NewsReader.Infrastructure;

public enum ENntpResponseCodes
{
    ConnectionReady = 200,
    ConnectionReadyButPostingNotAllowed = 201,
    ConnectionClosed = 205,
    SendPassword = 381,
    AuthSuccess = 281,
    CommandNotRecognised = 500,
    SyntaxError = 501,
    AccessRestricted = 502,
}