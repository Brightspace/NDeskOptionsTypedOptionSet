
namespace NDesk.Options {

	partial class OptionSet<TArguments>
		where TArguments : new() {

		private abstract class OptionBuilder {

			internal abstract void Build(
					OptionSet optionSet,
					TArguments args
				);
		}

	}
}
