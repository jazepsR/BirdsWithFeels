using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetConversationTopic : MonoBehaviour
{

    Animator birdAnimator;
    public SpriteRenderer conversationTopicSprite;

    // Start is called before the first frame update
    void Start()
    {
        birdAnimator = GetComponent<Animator>();
    }


    //This is called from animations - when birds start talking
    public void updateConversationTopicBasedOnMood()
    {
        conversationTopicSprite.sprite = BirdConversationMatcher.Instance.getRelevantTopic(birdAnimator);


    }


}