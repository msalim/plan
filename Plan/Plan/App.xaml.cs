﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechRecognition;

namespace Plan
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        Frame rootFrame;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
        //var storageFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(
        //new Uri("ms-appx://VoiceCommandDefinition.xml"));
        //await Windows.ApplicationModel.VoiceCommands.VoiceCommandDefinitionManager.
        //InstallCommandDefinitionsFromStorageFileAsync(storageFile);

            Frame rootFrame = Window.Current.Content as Frame;
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        protected override void OnActivated(IActivatedEventArgs e)
        {
            
          // Was the app activated by a voice command?
          if (e.Kind != Windows.ApplicationModel.Activation.ActivationKind.VoiceCommand)
          {
            return;
          }

          var commandArgs = e as Windows.ApplicationModel.Activation.VoiceCommandActivatedEventArgs;

          SpeechRecognitionResult speechRecognitionResult = 
            commandArgs.Result;

          // Get the name of the voice command and the text spoken
          string voiceCommandName = speechRecognitionResult.RulePath[0];
          string textSpoken = speechRecognitionResult.Text;
          // The commandMode is either "voice" or "text", and it indicates how the voice command was entered by the user.
          // Apps should respect "text" mode by providing feedback in a silent form.
          string commandMode = this.SemanticInterpretation("commandMode", speechRecognitionResult);
        
          string topic = "";
          string time = "";



         switch (voiceCommandName)
                {
                    case "planner":
                        // Access the value of the {destination} phrase in the voice command
                        topic = speechRecognitionResult.SemanticInterpretation.Properties["topic"][0];
                        time = speechRecognitionResult.SemanticInterpretation.Properties["time"][0];
                        break;

                    default:
                        // There is no match for the voice command name. Navigate to MainPage
                        break;
                }

                // Repeat the same basic initialization as OnLaunched() above, taking into account whether
                // or not the app is already active.
                rootFrame = Window.Current.Content as Frame;
          if (this.rootFrame == null)
          {
            // App needs to create a new Frame, not shown
          }

          // Repeat the same basic initialization as OnLaunched() above, taking into account whether
                // or not the app is already active.
                rootFrame = Window.Current.Content as Frame;

                // Do not repeat app initialization when the Window already has content,
                // just ensure that the window is active
                if (rootFrame == null)
                {
                    // Create a Frame to act as the navigation context and navigate to the first page
                    rootFrame = new Frame();

                    rootFrame.NavigationFailed += OnNavigationFailed;

                    // Place the frame in the current Window
                    Window.Current.Content = rootFrame;
                }

                if (!rootFrame.Navigate(typeof(MainPage), topic))
                {
                    throw new Exception("Failed to create voice command page");
                }

                // Ensure the current window is active
                Window.Current.Activate();
            }
        /// <summary>
        /// Returns the semantic interpretation of a speech result. Returns null if there is no interpretation for
        /// that key.
        /// </summary>
        /// <param name="interpretationKey">The interpretation key.</param>
        /// <param name="speechRecognitionResult">The result to get an interpretation from.</param>
        /// <returns></returns>
        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }



    }
}
