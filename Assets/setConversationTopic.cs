using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setConversationTopic : MonoBehaviour
{

    Animator birdAnimator;
    public SpriteRenderer conversationTopicSprite;
    public script_birdAnim_makeTopicMatchMood moodPrefab;

    // Start is called before the first frame update
    void Start()
    {
        birdAnimator = GetComponent<Animator>();
    }


    //This is called from animations - when birds start talking
    public void updateConversationTopicBasedOnMood()
    {
        conversationTopicSprite.sprite = moodPrefab.getRelevantTopic(birdAnimator);
    }


}