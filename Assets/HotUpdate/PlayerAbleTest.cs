using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace HotUpdate
{
    public class PlayerAbleTest: MonoBehaviour
    {
        public AnimationClip IdleClip;
        public AnimationClip WalkClip;
        public AnimationClip RunClip;
        [Range(0, 1)] public float speed;
        private AnimationMixerPlayable _mixerPlayable;
        private void Start()
        {
            Test2();
        }
        void Update() {
            _mixerPlayable.SetInputWeight(0, 1.0f - speed);
            _mixerPlayable.SetInputWeight(1, speed);
        }

        public void Test1()
        {
            AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), IdleClip, out PlayableGraph graph);
        }

        public void Test2()
        {
            PlayableGraph graph = PlayableGraph.Create("test2Graph");
            var animationOutputPlayable = AnimationPlayableOutput.Create(graph, "AnimationOutput", GetComponent<Animator>());
            _mixerPlayable = AnimationMixerPlayable.Create(graph, 2);
            var walkClipPlayable = AnimationClipPlayable.Create(graph, WalkClip);
            var runClipPlayable = AnimationClipPlayable.Create(graph, RunClip);
            graph.Connect(walkClipPlayable, 0, _mixerPlayable, 0);
            graph.Connect(runClipPlayable, 0, _mixerPlayable, 1);
            animationOutputPlayable.SetSourcePlayable(_mixerPlayable);
            graph.Play();
        }
    }
}