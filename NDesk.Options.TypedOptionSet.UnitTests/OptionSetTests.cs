using System;
using System.IO;
using System.Text;

using NUnit.Framework;

namespace NDesk.Options.TypedOptionSet.UnitTests {

	[TestFixture]
	public sealed class OptionSetTests {

		private sealed class Args {

			public string Name;
			public int Age;
		}

		[Test]
		public void ActionWithoutDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add(
					"name=",
					( Args args, string value ) => { args.Name = value; }
				)
				.Add(
					"age=",
					( Args args, string value ) => { args.Age = Int32.Parse( value ); }
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--name=Jim", 
						"--age=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --name=VALUE           
      --age=VALUE            
" );
		}

		[Test]
		public void ActionDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add(
					"name=",
					"The name",
					( Args args, string value ) => { args.Name = value; }
				)
				.Add(
					"age=",
					"The age",
					( Args args, string value ) => { args.Age = Int32.Parse( value ); }
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--name=Jim", 
						"--age=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --name=VALUE           The name
      --age=VALUE            The age
" );
		}

		[Test]
		public void TypedActionWithoutDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add<string>(
					"name=",
					( Args args, string value ) => { args.Name = value; }
				)
				.Add<int>(
					"age=",
					( Args args, int value ) => { args.Age = value; }
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--name=Jim", 
						"--age=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --name=VALUE           
      --age=VALUE            
" );
		}

		[Test]
		public void TypedActionDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add<string>(
					"name=",
					"The name",
					( Args args, string value ) => { args.Name = value; }
				)
				.Add<int>(
					"age=",
					"The age",
					( Args args, int value ) => { args.Age = value; }
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--name=Jim", 
						"--age=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --name=VALUE           The name
      --age=VALUE            The age
" );
		}

		[Test]
		public void KeyedActionWithoutDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add(
					"person=",
					( Args args, string key, string value ) => {
						args.Name = key;
						args.Age = Int32.Parse( value );
					}
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--person", 
						"Jim=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --person=VALUE1:VALUE2 
" );
		}

		[Test]
		public void KeyedActionWithDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add(
					"person=",
					"The person's name and age",
					( Args args, string key, string value ) => {
						args.Name = key;
						args.Age = Int32.Parse( value );
					}
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--person", 
						"Jim=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --person=VALUE1:VALUE2 The person's name and age
" );
		}

		[Test]
		public void TypedKeyedActionWithoutDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add<string, int>(
					"person=",
					( Args args, string key, int value ) => {
						args.Name = key;
						args.Age = value;
					}
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--person", 
						"Jim=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --person=VALUE1:VALUE2 
" );
		}

		[Test]
		public void TypedKeyedActionWithDescription() {

			OptionSet<Args> optionSet = new OptionSet<Args>();
			optionSet
				.Add<string, int>(
					"person=",
					"The person's name and age",
					( Args args, string key, int value ) => {
						args.Name = key;
						args.Age = value;
					}
				);

			Args parsed = optionSet.Parse(
					new[] {
						"--person", 
						"Jim=34" 
					}
				);

			Assert.AreEqual( "Jim", parsed.Name );
			Assert.AreEqual( 34, parsed.Age );

			string descriptions = WriteOptionDescriptions( optionSet );

			AssertDescriptions( optionSet,
@"      --person=VALUE1:VALUE2 The person's name and age
" );
		}

		private static void AssertDescriptions<T>(
				OptionSet<T> optionSet,
				string exceptedDescriptions
			)
			where T : new() {

			string descriptions = WriteOptionDescriptions( optionSet );
			Assert.AreEqual( exceptedDescriptions, descriptions );
		}

		private static string WriteOptionDescriptions<T>( OptionSet<T> optionSet )
			where T : new() {

			StringBuilder descriptions = new StringBuilder();

			using( StringWriter sw = new StringWriter( descriptions ) ) {
				optionSet.WriteOptionDescriptions( sw );
			}

			return descriptions.ToString();
		}

	}
}
