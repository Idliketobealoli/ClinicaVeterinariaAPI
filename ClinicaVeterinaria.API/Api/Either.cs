namespace ClinicaVeterinaria.API.Api
{
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

        public T Match<T>(Func<TSuccess, T> onSuccess, Func<TError, T> onError)
        {
            return _isSuccess ? onSuccess(_successValue) : onError(_errorValue);
        }
    }
}
