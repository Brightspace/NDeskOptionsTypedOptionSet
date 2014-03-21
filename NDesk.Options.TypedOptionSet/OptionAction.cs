
namespace NDesk.Options {

	public delegate void OptionAction<TArguments, TKey, TValue>(
			TArguments arguments,
			TKey key,
			TValue value
		);
}
