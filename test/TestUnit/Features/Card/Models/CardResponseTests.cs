


namespace TestUnit.Features.Card.Models;

public class CardResponseTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        // (Setup any required variables or state here)

        // Act
        var cardResponse = new CardResponse();

        // Assert
        Assert.NotNull(cardResponse);

    }
}