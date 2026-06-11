using System.ComponentModel;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace CosmicPizzaServer.Prompts;

[McpServerPromptType]
public static class PartyPlannerPrompt
{
    [McpServerPrompt]
    [Description(
        "Helps plan an epic space pizza party with "
        + "menu suggestions, activities, and cosmic "
        + "decorations.")]
    public static ChatMessage[] PlanParty(
        [Description(
            "Number of guests attending")]
        string guestCount,
        [Description(
            "Planet where the party will be held")]
        string planet,
        [Description(
            "Type of occasion: birthday, " +
            "graduation, or just-because")]
        string occasion)
    {
        var systemMessage = new ChatMessage(
            ChatRole.System,
            "You are the official Cosmic Pizza " +
            "Party Planner, an enthusiastic and " +
            "creative AI that helps organize " +
            "unforgettable space-themed pizza " +
            "parties. You know everything about " +
            "the Cosmic Pizza menu: The Mars " +
            "Margherita, Saturn's Ring-ion Burger " +
            "Pizza, Neptune's Deep Sea Supreme, " +
            "Jupiter's Gas Giant Garlic Bread " +
            "Pizza, and Pluto's Dwarf Delight. " +
            "Use space puns and cosmic humor. " +
            "Include pizza quantities, drink " +
            "suggestions (Milky Way Milkshakes, " +
            "Asteroid Punch, etc.), decoration " +
            "ideas, and fun activities. Always " +
            "be upbeat and enthusiastic!");

        var userMessage = new ChatMessage(
            ChatRole.User,
            $"I am planning a {occasion} party on " +
            $"{planet} for {guestCount} guests. " +
            "Please help me plan the perfect " +
            "cosmic pizza party! Include:\n" +
            "1. How many of each pizza to order\n" +
            "2. Drink recommendations\n" +
            "3. Decoration ideas that fit the " +
            "planet's theme\n" +
            "4. Fun space-themed party activities\n" +
            "5. A suggested party schedule\n" +
            "6. The estimated total in GalactiCoins");

        return [systemMessage, userMessage];
    }
}
