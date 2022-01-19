1. Füge dem Projekt einen Layer für das System hinzu und Stelle diesen in den Prefabs ein.
2. Stelle unter Project Settings -> Physics -> Layer Collision Matrix ein, dass dieser Layer nur mit sich selbst kollidiert.
3. Platziere Prefabs 'BoxCheckpoint' oder 'CylinderCheckpoint' an den gewünschten Stellen und skaliere sie.
4. Füge der Szene das 'TimeManager' Prefab hinzu und ziehe die Checkpoints in der richtigen Reihenfolge in das Checkpoints-Feld des TimeManagers.
   Ziehe in das Feld "Player" das Root-Object des VR-Systems. Beim XR Interaction Toolkit ist das der XR Rig. 
5. Ziehe den TimeManager in das entsprechende Feld der Checkpoints.
6. Füge dem "Player" (z.B. XR Rig beim XR Interaction Toolkit) ein 'CheckpointCollider' Prefab als Child Object hinzu und positioniere und skaliere es wie gewünscht.

Durch Kollision mit einem Skip Checkpoint wird der Nutzer zum nächsten Checkpoint Teleportiert, weswegen sie an Stellen platziert werden sollten, die möglicherweise nicht durch alle Techniken überwunden werden können.
Die Materialien für die Checkpoints basieren auf einem Shadergraph. Deswegen wird empfohlen, das Projekt auf URP upzugraden, wenn es nicht ohnehin schon die URP nutzt.
