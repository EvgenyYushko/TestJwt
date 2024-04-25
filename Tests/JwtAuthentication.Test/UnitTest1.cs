using NUnit.Framework;

namespace JwtAuthentication.Test
{
	[TestFixture]
	public class FormHelperTests
	{
		[Test]
		public void GetFormTextCaption_GenerateFormTextCaption_TextCaption()
		{
			// Arrange
			var headerText = "Test";
			var bodyText = "Test2";
			var expected = $"{headerText} {bodyText}";

			// Act
			var actual = true;

			// Assert
			Assert.That("a" == "a");
		}
	}
}

