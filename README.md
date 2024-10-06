## Arcanoid-shooter prototype
It took me a day and a half to develop this prototype.<br>
The main focus is on working with bullets. They can fly very fast and there can be a lot of them. Regardless of the fps, the distance between the bullets will be the same. As the rate of fire increases, the distance decreases.

![1](https://github.com/user-attachments/assets/de68dcf9-b053-41c1-b459-19c406980237)

To store the bullets, I used a structure that contains the bullet spawn position and the time stamp when the bullet appeared. This data is enough to draw all the bullets through the API `Graphics.DrawMeshInstanced`.<br>
The screen shows the current number of bullets and the frame counter.<br> 
Even with 7-10 thousand bullets, the game plays at 60 fps on low devices.<br>
All code is available for public use.

![2](https://github.com/user-attachments/assets/14f9464a-1f0f-4d86-a4f9-bca71bd247b1)

Sample code for handling bullets. There is a list of packs, each pack has up to 1023 bullets. Such implementation is related to the limitation of `Graphics.DrawMeshInstanced` to process up to 1023 objects at once.
```cs
        private void ProcessBullets()
        {
            _currentBulletAmount = 0;

            for (var i = 0; i < _bulletPacks.Count; i++)
            {
                var pack = _bulletPacks[i];
                _currentBulletAmount += pack.Count;
                ProcessPack(pack);
            }
        }

        private void ProcessPack(List<BulletData> pack)
        {
            _matrices.Clear();
            var raycastDistance = Time.deltaTime * BulletSpeed;
            RaycastHit2D rayHit;

            for (var i=0;i<pack.Count;i++)
            {
                var data = pack[i];
                var passedTime = Time.time - data.SpawnTime;
                var dist = passedTime * BulletSpeed;
                var pos = data.Position + Vector3.up * dist;

                if (dist > _maxScreenHeight)
                {
                    pack[i] = data.Die();
                } else
                {
                    rayHit = Physics2D.Raycast(pos, Vector2.up, raycastDistance, hitLayer);
                    if(rayHit.collider != null && rayHit.collider.TryGetComponent(out Block block))
                    {
                        block.DoHit();
                        pack[i] = data.Explode();
                        SoundManager.Instance.Hit();
                    }
                }

                _matrices.Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
            }

            Graphics.DrawMeshInstanced(_bulletMesh, 0, bulletMaterial, _matrices);

            for (var i = pack.Count - 1; i >= 0; i--)
            {
                var data = pack[i];
                if (data.Died)
                    pack.RemoveAt(i);
            }
        }
```
