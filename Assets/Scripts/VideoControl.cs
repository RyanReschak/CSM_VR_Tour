
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
namespace YoutubePlayer
{

    public class VideoControl : MonoBehaviour
    {
        public YoutubePlayer youtubePlayer;
        VideoPlayer videoPlayer;
        public Button bt_play;
        public Button bt_pause;
        public Button bt_reset;


        private void Awake()
        {
            bt_play.interactable = false;
            bt_pause.interactable = false;
            bt_reset.interactable = false;
            videoPlayer = youtubePlayer.GetComponent<VideoPlayer>();
            videoPlayer.prepareCompleted += VideoPlayerPreparedCompleted;
        }

        void Start()
        {
            Prepare();
        }

        void VideoPlayerPreparedCompleted(VideoPlayer source)
        {
            bt_play.interactable = source.isPrepared;
            bt_pause.interactable = source.isPrepared;
            bt_reset.interactable = source.isPrepared;
        }

        public async void Prepare()
        {
            print("Preparing Video...");
            try
            {
                await youtubePlayer.PrepareVideoAsync();
                print("Video Prepared");
                videoPlayer.Play();
            }
            catch
            {
                print("ERROR Preparing Video");
            }
        }

        public void PlayVideo()
        {
            videoPlayer.Play();
        }

        public void PauseVideo()
        {
            videoPlayer.Pause();
        }

        public void ResetVideo()
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }

        void OnDestroy()
        {
            videoPlayer.prepareCompleted -= VideoPlayerPreparedCompleted;
        }
    }

}
