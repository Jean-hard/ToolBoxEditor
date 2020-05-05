using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

namespace ToolboxEngine
{
    [ExecuteAlways]
    public class TextureRepeatQuad : MonoBehaviour
    {
        private Vector3 _currentLocalScale = Vector3.one;

        private Renderer _renderer = null;
        private MeshFilter _meshFilter = null;

        void Start()
        {
            _currentLocalScale = transform.localScale;
            _renderer = GetComponent<Renderer>();
            _meshFilter = GetComponent<MeshFilter>();
            _RefreshMesh();
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.localScale == _currentLocalScale) return;
            _currentLocalScale = transform.localScale;

            _RefreshMesh();
        }

        private void _RefreshMesh()
        {
            Mesh mesh = _GetMesh();
            if (null == mesh) return;

            mesh.uv = SetupUVMap();

            if (_renderer.sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat)
            {
                _renderer.sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }
        }

        private Mesh _GetMesh()
        {
            if (Application.isPlaying)
            {
                _meshFilter.mesh.name = "Mesh" + GetInstanceID();
                return _meshFilter.mesh;
            }
            else
            {
                if (null == _meshFilter) return null;
                if (null == _meshFilter.sharedMesh) return null;

                string meshId = "Mesh" + GetInstanceID();
                if (_meshFilter.sharedMesh.name != meshId)
                {
                    Mesh meshCopy = Instantiate(_meshFilter.sharedMesh);
                    meshCopy.name = meshId;
                    _meshFilter.sharedMesh = meshCopy;
                }

                return _meshFilter.sharedMesh;
            }
        }

        private Vector2[] SetupUVMap()
        {
            float scaleX = transform.localScale.x;
            float scaleY = transform.localScale.y;

            Vector2[] meshUVs = new Vector2[4];

            meshUVs[0] = new Vector2(0f, 0f);
            meshUVs[1] = new Vector2(scaleX, 0f);
            meshUVs[2] = new Vector2(0f, scaleY);
            meshUVs[3] = new Vector2(scaleX, scaleY);

            return meshUVs;
        }
    }
}
