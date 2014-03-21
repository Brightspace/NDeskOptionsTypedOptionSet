using System;

namespace NDesk.Options {

	partial class OptionSet<TArguments>
		where TArguments : new() {

		private sealed class KeyedActionOptionBuilder : OptionBuilder {

			private readonly string m_prototype;
			private readonly string m_description;
			private readonly OptionAction<TArguments, string, string> m_action;

			internal KeyedActionOptionBuilder(
					string prototype,
					string description,
					OptionAction<TArguments, string, string> action
				) {

				m_prototype = prototype;
				m_description = description;
				m_action = action;
			}

			internal override void Build( OptionSet optionSet, TArguments args ) {

				optionSet.Add(
						m_prototype,
						m_description,
						( string key, string value ) => {
							m_action( args, key, value );
						}
					);
			}
		}

		private sealed class KeyedActionOptionBuilder<TKey, TValue> : OptionBuilder {

			private readonly string m_prototype;
			private readonly string m_description;
			private readonly OptionAction<TArguments, TKey, TValue> m_action;

			internal KeyedActionOptionBuilder(
					string prototype,
					string description,
					OptionAction<TArguments, TKey, TValue> action
				) {

				m_prototype = prototype;
				m_description = description;
				m_action = action;
			}

			internal override void Build( OptionSet optionSet, TArguments args ) {

				optionSet.Add<TKey, TValue>(
						m_prototype,
						m_description,
						( TKey key, TValue value ) => {
							m_action( args, key, value );
						}
					);
			}
		}

	}
}
