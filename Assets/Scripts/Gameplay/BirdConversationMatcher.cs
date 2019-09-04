using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdConversationMatcher : MonoBehaviour
{
    public static BirdConversationMatcher Instance { get; private set; }

    public Sprite[] neutralConversationSymbols;
    public Sprite[] ConfidentConversationSymbols;
    public Sprite[] CautiousConversationSymbols;
    public Sprite[] SolitaryConversationSymbols;
    public Sprite[] SocialConversationSymbols;
    
    int emotion;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public Sprite getRelevantTopic(Animator birdAnimator)
    {
       

        emotion = birdAnimator.GetInteger("emotion");
        switch (emotion)
        {
            case 0:
                return returnRandomSpriteInCollection(neutralConversationSymbols);
               

            case 1:
                return returnRandomSpriteInCollection(CautiousConversationSymbols);
                

            case 2:
                return returnRandomSpriteInCollection(ConfidentConversationSymbols);
             

            case 3:
                return returnRandomSpriteInCollection(SocialConversationSymbols);
                

            case 4:
                return returnRandomSpriteInCollection(ConfidentConversationSymbols);
               

        }

        print("No sprite found");
        return null;

    }

    Sprite returnRandomSpriteInCollection(Sprite[] spriteCollection)
    {
 

        return spriteCollection[0];

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
