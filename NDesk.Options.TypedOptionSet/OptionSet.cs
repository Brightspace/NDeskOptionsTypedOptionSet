using System;
using System.Collections.Generic;
using System.IO;

namespace NDesk.Options {

	public sealed partial class OptionSet<TArguments>
		where TArguments : new() {

		private readonly List<OptionBuilder> m_optionBuilders = new List<OptionBuilder>();
		private readonly Converter<string, string> m_localizer;

		public OptionSet() {
			m_localizer = null;
		}

		public OptionSet( Converter<string, string> localizer ) {
			m_localizer = localizer;
		}

		#region Without Description

		public OptionSet<TArguments> Add(
				string prototype,
				Action<TArguments, string> action
			) {

			return this.Add(
					prototype: prototype,
					description: null,
					action: action
				);
		}

		public OptionSet<TArguments> Add<TValue>(
				string prototype,
				Action<TArguments, TValue> action
			) {

			return this.Add<TValue>(
					prototype: prototype,
					description: null,
					action: action
				);
		}

		public OptionSet<TArguments> Add(
				string prototype,
				OptionAction<TArguments, string, string> action
			) {

			return this.Add(
					prototype: prototype,
					description: null,
					action: action
				);
		}

		public OptionSet<TArguments> Add<TKey, TValue>(
				string prototype,
				OptionAction<TArguments, TKey, TValue> action
			) {

			return this.Add<TKey, TValue>(
					prototype: prototype,
					description: null,
					action: action
				);
		}

		#endregion

		#region With Description

		public OptionSet<TArguments> Add(
				string prototype,
				string description,
				Action<TArguments, string> action
			) {

			OptionBuilder builder = new ActionOptionBuilder(
					prototype,
					description,
					action
				);

			m_optionBuilders.Add( builder );
			return this;
		}

		public OptionSet<TArguments> Add<TValue>(
				string prototype,
				string description,
				Action<TArguments, TValue> action
			) {

			OptionBuilder builder = new ActionOptionBuilder<TValue>(
					prototype,
					description,
					action
				);

			m_optionBuilders.Add( builder );
			return this;
		}

		public OptionSet<TArguments> Add(
				string prototype,
				string description,
				OptionAction<TArguments, string, string> action
			) {

			OptionBuilder builder = new KeyedActionOptionBuilder(
					prototype,
					description,
					action
				);

			m_optionBuilders.Add( builder );
			return this;
		}

		public OptionSet<TArguments> Add<TKey, TValue>(
				string prototype,
				string description,
				OptionAction<TArguments, TKey, TValue> action
			) {

			OptionBuilder builder = new KeyedActionOptionBuilder<TKey, TValue>(
					prototype,
					description,
					action
				);

			m_optionBuilders.Add( builder );
			return this;
		}

		#endregion

		public TArguments Parse(
				IEnumerable<string> arguments
			) {

			List<string> extras;
			return this.Parse( arguments, out extras );
		}

		public TArguments Parse(
				IEnumerable<string> arguments,
				out List<string> extras
			) {

			OptionSet set = CreateOptionSet();
			TArguments args = new TArguments();

			BuildOptionSet( set, args );

			extras = set.Parse( arguments );
			return args;
		}

		public void WriteOptionDescriptions( TextWriter o ) {

			OptionSet set = CreateOptionSet();
			TArguments args = new TArguments();

			BuildOptionSet( set, args );

			set.WriteOptionDescriptions( o );
		}

		private OptionSet CreateOptionSet() {

			if( m_localizer == null ) {
				return new OptionSet();
			} else {
				return new OptionSet( m_localizer );
			}
		}

		private void BuildOptionSet(
				OptionSet set,
				TArguments args
			) {

			foreach( OptionBuilder builder in m_optionBuilders ) {
				builder.Build( set, args );
			}
		}
	}

}
