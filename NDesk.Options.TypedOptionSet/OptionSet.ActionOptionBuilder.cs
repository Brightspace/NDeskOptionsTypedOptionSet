using System;

namespace NDesk.Options {

	partial class OptionSet<TArguments>
		where TArguments : new() {

		private sealed class ActionOptionBuilder : OptionBuilder {

			private readonly string m_prototype;
			private readonly string m_description;
			private readonly Action<TArguments, string> m_action;

			internal ActionOptionBuilder(
					string prototype,
					string description,
					Action<TArguments, string> action
				) {

				m_prototype = prototype;
				m_description = description;
				m_action = action;
			}

			internal override void Build( OptionSet optionSet, TArguments args ) {

				optionSet.Add(
						m_prototype,
						m_description,
						( string value ) => {
							m_action( args, value );
						}
					);
			}
		}

		private sealed class ActionOptionBuilder<TValue> : OptionBuilder {

			private readonly string m_prototype;
			private readonly string m_description;
			private readonly Action<TArguments, TValue> m_action;

			internal ActionOptionBuilder(
					string prototype,
					string description,
					Action<TArguments, TValue> action
				) {

				m_prototype = prototype;
				m_description = description;
				m_action = action;
			}

			internal override void Build( OptionSet optionSet, TArguments args ) {

				optionSet.Add<TValue>(
						m_prototype,
						m_description,
						( TValue value ) => {
							m_action( args, value );
						}
					);
			}
		}

	}
}
