using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using ScriptPortal.Vegas;

namespace RenderProject
{
    public class EntryPoint
    {
        public void FromVegas(Vegas myVegas)
        {
            ScriptArgs args = ScriptPortal.Vegas.Script.Args;

            if (! areArgsValid(args)) {
                myVegas.Exit();
                return;
            }

            try
            {
                myVegas.NewProject();

                VideoTrack videoTrack = new VideoTrack(myVegas.Project, 0, "Video");
                myVegas.Project.Tracks.Add(videoTrack);

                AudioTrack audioTrack = new AudioTrack(myVegas.Project, 1, "Audio");
                myVegas.Project.Tracks.Add(audioTrack);

                string videoPath = args.ValueOf("videoPath");
                VideoEvent imageEvent = (VideoEvent)AddMedia(
                    myVegas.Project,
                    videoPath,
                    0,
                    Timecode.FromSeconds(0)
                );

                string audioPath = videoPath;

                AudioEvent audioEvent = (AudioEvent)AddMedia(
                    myVegas.Project,
                    audioPath,
                    1,
                    Timecode.FromSeconds(0)
                );

                myVegas.SaveProject(args.ValueOf("projectFilePath"));
                myVegas.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        TrackEvent AddMedia(
            Project project,
            string mediaPath,
            int trackIndex,
            Timecode start
        )
        {
            Media media = Media.CreateInstance(project, mediaPath);
            Timecode length = media.Length;
            Track track = project.Tracks[trackIndex];

            if (track.MediaType == MediaType.Video)
            {
                VideoTrack videoTrack = (VideoTrack)track;
                VideoEvent videoEvent = videoTrack.AddVideoEvent(start, length);
                Take take = videoEvent.AddTake(media.GetVideoStreamByIndex(0));

                return videoEvent;
            }
            else if (track.MediaType == MediaType.Audio)
            {
                AudioTrack audioTrack = (AudioTrack)track;
                AudioEvent audioEvent = audioTrack.AddAudioEvent(start, length);
                Take take = audioEvent.AddTake(media.GetAudioStreamByIndex(0));

                return audioEvent;
            }
            
            // Should be impossible
            return null;
        }

        private bool areArgsValid(ScriptArgs args)
        {
            return
                args.Exists("projectFilePath")
                && args.Exists("videoPath")
                && args.Exists("outputFilePath")
            ;
        } 
    }
}

