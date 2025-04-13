# Unit Testing - Natalie Morales

I created 3 unit tests which include the following:

1. **ScoreManagerTest**
   - Verifies stroke count increments correctly.
   - Ensures overall score adds up accurately across levels.
   - Confirms score text updates when strokes are changed.

2. **SettingsManagerTest**
   - Tests volume values are clamped correctly (0.0001 to 1).
   - Verifies PlayerPrefs saves the volume settings.
   - Confirms the correct float dB value is applied to the AudioMixer.

3. **SoundEffectsTest**
   - Checks if sound effect prefabs instantiate and play.
   - Verifies audio sources auto-destroy after playing.
   - Tests SFXManager does not duplicate sound effects.


- All tests executed via Unity Test Runner.
