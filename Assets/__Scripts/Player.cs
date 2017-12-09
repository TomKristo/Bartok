using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Enables LINQ queries, which will be explained soon

// The player can either be human or an ai
public enum PlayerType
{
    human,
    ai
}

// The individual player of the game
// Note: Player does NOT extend MonoBehaviour (or any other class)

[System.Serializable] // Make the Player class visible in the Inspector pane
public class Player
{

    public PlayerType type = PlayerType.ai;
    public int playerNum;

    public List<CardBartok> hand; // The cards in this player's hand

    public SlotDef handSlotDef;

    // Add a card to the hand
    public CardBartok AddCard(CardBartok eCB)
    {
        if (hand == null) hand = new List<CardBartok>();

        // Add the card to the hand
        hand.Add(eCB);
        FanHand();
        return (eCB);
    }

    // Remove a card from the hand
    public CardBartok RemoveCard(CardBartok cb)
    {
        hand.Remove(cb);
        FanHand();
        return (cb);
    }

    public void FanHand()
    {
        float startRot = 0;
        startRot = handSlotDef.rot;
        if (hand.Count > 1)
        {
            startRot += Bartok.S.handFanDegrees * (hand.Count - 1) / 2;
        }

        Vector3 pos;
        float rot;
        Quaternion rotQ;
        for (int i = 0; i < hand.Count; i++)
        {
            rot = startRot - Bartok.S.handFanDegrees * i;
            rotQ = Quaternion.Euler(0, 0, rot);
            pos = Vector3.up * CardBartok.CARD_HEIGHT / 2f;

            pos = rotQ * pos;

            pos += handSlotDef.pos;
            pos.z = -0.5f * i;

            hand[i].transform.localPosition = pos;
            hand[i].transform.rotation = rotQ;
            hand[i].state = CBState.hand;

            hand[i].faceUp = (type == PlayerType.human);

            hand[i].SetSortOrder(i * 4);
        }
    }
}