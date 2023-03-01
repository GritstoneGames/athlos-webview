# Athlos Gameface View Helper
This package provides a helpful component to streamline integration with Athlos via a Coherent Labs Gameface view.

## Instructions
1. Attach the AthlosGamefaceView component to any game object within the same scene as the Coherent Labs Gameface view.
2. Set the `View` variable to the Gameface View in your scene.
3. Follow the common instructions as documented [here](https://athlos.readme.io/docs).

If embedding the Gameface view within a canvas hierarchy to make it a subiew of your existing user interface:
1. Attach a RawImage component to where you want Athlos to appear
2. Attach the GamefaceRenderToCanvas to your Camera object.
3. Set the `View` variable to your AthlosGamefaceView component
4. Set the `Image` variable to your RawImage component
