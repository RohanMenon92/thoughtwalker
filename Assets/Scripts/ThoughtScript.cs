﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace GGJ.Thoughts {
    public class ThoughtScript : MonoBehaviour {
        Sequence sequence;

        bool autoFade;
        string thoughtText;
        float disappearDelay;
        string scramble;
        float speed;

        public void Awake() {
            sequence = DOTween.Sequence();
        }

        // Use this for initialization
        public Sequence PlayThought(ThoughtData thoughtData) {
            SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
            Color spriteColor = sRenderer.color;
            spriteColor.a = 0f;
            sRenderer.color = spriteColor;
            sRenderer.sprite = thoughtData.thoughtSprite;
            thoughtText = thoughtData.message;
            GetComponentInChildren<TextMeshPro>().text = "";
            disappearDelay = thoughtData.fadeDelay;
            autoFade = thoughtData.autoFade;
            scramble = thoughtData.scramble;
            speed = thoughtData.speed;

            TextMeshPro textMesh = GetComponentInChildren<TextMeshPro>();
            sRenderer.DOFade(1.0f, 1.0f);

            if(scramble == string.Empty) {
                textMesh.text = thoughtText;
                sequence.Append(textMesh.DOFade(1.0f, 1.0f).SetEase(Ease.OutQuad));
            } else {
                int s = 0;
                textMesh.DOFade(1.0f, 1.5f);
                sequence.Append(DOTween.To(() => s, strLen => {
                    if(scramble != string.Empty) {
                        string newText = thoughtText.Substring(0, strLen) + scramble.Substring(strLen, thoughtText.Length - strLen);
                        textMesh.text = newText;
                    } else {
                        textMesh.text = thoughtText.Substring(0, strLen);
                    }
                }, thoughtText.Length, thoughtText.Length / speed).SetEase(Ease.Linear));
            }
            return sequence;
        }

        public void FadeThought() {
            transform.parent = null;
            SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
            GetComponentInChildren<TextMeshPro>().DOFade(0.0f, 0.5f);
            Tweener tweenFade = sRenderer.DOFade(0.0f, 0.5f).OnComplete(() => KillThought()).SetAutoKill();
        }

        void KillThought() {
            GameObject.Destroy(gameObject);
        }
    }
}