using ApplesToApples.GameClasses;
using ApplesToApples.Players.Interfaces;
using StreamingDataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text.Json;

public class RealPlayer : IPlayer, IRealPlayer
{
    public int PlayerID { get; init;}
    public List<string> Hand { get; init; }

    private StreamReader inFromClient;
    private StreamWriter outToClient;
    private int score;


    public RealPlayer(int playerID, List<string> hand, StreamReader inFromClient, StreamWriter outToClient)
    {
        PlayerID = playerID;
        this.Hand = hand;
        this.inFromClient = inFromClient;
        this.outToClient = outToClient;
    }

    public PlayedApple? Play()
    {
       
        try
        {

            string json = inFromClient.ReadLine()!;
            var playedCard = JsonSerializer.Deserialize<PlayedApple>(json)!;

            Apples2Apples.PlayedApple!.Add(playedCard);
            return playedCard;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore nella ricezione del voto: {ex.Message}");
            return null;
        }

        
    }

    public PlayedApple Judge()
    {
        int playedAppleIndex = 0;
        try
        {
            playedAppleIndex = int.Parse(inFromClient.ReadLine()!);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return Apples2Apples.PlayedApple![playedAppleIndex];

    }

    public void AddCard(string redApple)
    {
        try
        {
            ClientServerMessages<string> messageToClient = new ClientServerMessages<string>(1, redApple);
            string jsonData = JsonSerializer.Serialize(messageToClient);
            outToClient.WriteLine(jsonData);
            outToClient.Flush();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void SetFirstRound(HandDataObjects handData)
    {
        try
        {
            ClientServerMessages<HandDataObjects> messageToClient = new ClientServerMessages<HandDataObjects>(5, handData);
            string jsonData = JsonSerializer.Serialize(messageToClient);
            outToClient.WriteLine(jsonData);
            outToClient.Flush();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void GetCardList() {
        ClientServerMessages<List<PlayedApple>> messageToClient = new ClientServerMessages<List<PlayedApple>>(2, Apples2Apples.PlayedApple!);
        string jsonData = JsonSerializer.Serialize(messageToClient);
        outToClient.WriteLine(jsonData);
        outToClient.Flush();

    }

    public void GetEndMessage(int id) { 
        score = 0;
        ClientServerMessages<string> messageToClient = new ClientServerMessages<string>(3, $"{id}");
        string jsonData = JsonSerializer.Serialize(messageToClient);
        outToClient.WriteLine(jsonData);
        outToClient.Flush();
    }

    public void ResetRound() {
        ClientServerMessages<string> messageToClient = new ClientServerMessages<string>(9, "reset");
        string jsonData = JsonSerializer.Serialize(messageToClient);
        outToClient.WriteLine(jsonData);
        outToClient.Flush();
    }

    public int GetScore()
    {
        return score;
    }

    public void IncrementScore()
    {
        score++;
    }

   
}

