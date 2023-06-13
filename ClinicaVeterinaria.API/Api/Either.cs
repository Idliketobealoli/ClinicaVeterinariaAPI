namespace ClinicaVeterinaria.API.Api
{
    #region source
    // To create this class, we followed the guidance of a comment made by Zoran Horvat in
    // https://stackoverflow.com/questions/63231450/how-to-use-the-either-type-in-c
    // His comment explained in a lot of detail the basics of ROP and the thread in general was very useful.
    #endregion
    // This class will be the core of our Railway Oriented Programming implementation.
    public class Either<TSuccess, TError>
    {
        public readonly TSuccess? _successValue;
        public readonly TError? _errorValue;
        public readonly bool _isSuccess;

        public Either(TSuccess successValue)
        {
            _successValue = successValue;
            _isSuccess = true;
        }

        public Either(TError errorValue)
        {
            _errorValue = errorValue;
            _isSuccess = false;
        }

        // This function will be used to determine a different output depending on whether there was a successful result or not.
        public T Match<T>(Func<TSuccess, T> onSuccess, Func<TError, T> onError)
        {
            return _isSuccess ? onSuccess(_successValue) : onError(_errorValue);
        }
    }
}
