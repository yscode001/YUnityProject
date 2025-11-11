using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Tilemaps;
using UnityEngine.Video;

namespace YUnity
{
    #region 常用缓存属性
    public partial class MonoBehaviourBaseY : MonoBehaviour
    {
        private Camera _mainCamera;
        private Transform _mainCameraTransform;

        private Transform _transform;
        private RectTransform _rectTransform;
        private GameObject _gameObject;
        private PlayableDirector _playableDirector;
        private Animation _animation;
        private Animator _animator;
        private VideoPlayer _videoPlayer;
        private AudioSource _audioSource;
        private AudioListener _audioListener;
        private CharacterController _characterController;
        private Rigidbody _rigidbody;
        private Rigidbody2D _rigidbod2Dy;
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Renderer _renderer;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private SpriteRenderer _spriteRenderer;
        private TrailRenderer _trailRenderer;
        private LineRenderer _lineRenderer;
        private NavMeshAgent _navMeshAgent;
        private NavMeshObstacle _navMeshObstacle;
        private BoxCollider _boxCollider;
        private BoxCollider2D _boxCollider2D;
        private CapsuleCollider _capsuleCollider;
        private CapsuleCollider2D _capsuleCollider2D;
        private CircleCollider2D _circleCollider2D;
        private EdgeCollider2D _edgeCollider2D;
        private SpringJoint _springJoint;
        private SpringJoint2D _springJoint2D;
        private TilemapCollider2D _tilemapCollider2D;
        private CompositeCollider2D _compositeCollider2D;
        private HumanBodyBoneUtil _humanBodyBoneUtil;
    }
    public partial class MonoBehaviourBaseY
    {
        /// <summary>
        /// 主相机
        /// </summary>
        public Camera MainCameraY
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }
                return _mainCamera;
            }
        }

        /// <summary>
        /// 主相机的transform
        /// </summary>
        public Transform MainCameraTransformY
        {
            get
            {
                if (_mainCameraTransform == null && Camera.main != null)
                {
                    _mainCameraTransform = Camera.main.transform;
                }
                return _mainCameraTransform;
            }
        }

        /// <summary>
        /// 缓存属性
        /// </summary>
        public Transform TransformY
        {
            get
            {
                if (_transform != null) { return _transform; }
                _transform = transform;
                return _transform;
            }
        }

        /// <summary>
        /// 缓存属性，UI专用
        /// </summary>
        public RectTransform RectTransformY
        {
            get
            {
                if (_rectTransform != null) { return _rectTransform; }
                _rectTransform = gameObject.GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        /// <summary>
        /// 缓存属性
        /// </summary>
        public GameObject GameObjectY
        {
            get
            {
                if (_gameObject != null) { return _gameObject; }
                _gameObject = gameObject;
                return _gameObject;
            }
        }

        /// <summary>
        /// 获取PlayableDirector，如果没有则添加
        /// </summary>
        public PlayableDirector PlayableDirectorY
        {
            get
            {
                if (_playableDirector != null) { return _playableDirector; }
                _playableDirector = gameObject.GetComponent<PlayableDirector>();
                if (_playableDirector != null) { return _playableDirector; }
                _playableDirector = gameObject.AddComponent<PlayableDirector>();
                return _playableDirector;
            }
        }

        /// <summary>
        /// 获取Animation，如果没有则添加
        /// </summary>
        public Animation AnimationY
        {
            get
            {
                if (_animation != null) { return _animation; }
                _animation = gameObject.GetComponent<Animation>();
                if (_animation != null) { return _animation; }
                _animation = gameObject.AddComponent<Animation>();
                return _animation;
            }
        }

        /// <summary>
        /// 获取Animator，如果没有则添加
        /// </summary>
        public Animator AnimatorY
        {
            get
            {
                if (_animator != null) { return _animator; }
                _animator = gameObject.GetComponent<Animator>();
                if (_animator != null) { return _animator; }
                _animator = gameObject.AddComponent<Animator>();
                return _animator;
            }
        }

        /// <summary>
        /// 获取VideoPlayer，如果没有则添加
        /// </summary>
        public VideoPlayer VideoPlayerY
        {
            get
            {
                if (_videoPlayer != null) { return _videoPlayer; }
                _videoPlayer = gameObject.GetComponent<VideoPlayer>();
                if (_videoPlayer != null) { return _videoPlayer; }
                _videoPlayer = gameObject.AddComponent<VideoPlayer>();
                return _videoPlayer;
            }
        }

        /// <summary>
        /// 获取AudioSource，如果没有则添加
        /// </summary>
        public AudioSource AudioSourceY
        {
            get
            {
                if (_audioSource != null) { return _audioSource; }
                _audioSource = gameObject.GetComponent<AudioSource>();
                if (_audioSource != null) { return _audioSource; }
                _audioSource = gameObject.AddComponent<AudioSource>();
                return _audioSource;
            }
        }

        /// <summary>
        /// 获取AudioListener，如果没有则添加
        /// </summary>
        public AudioListener AudioListenerY
        {
            get
            {
                if (_audioListener != null) { return _audioListener; }
                _audioListener = gameObject.GetComponent<AudioListener>();
                if (_audioListener != null) { return _audioListener; }
                _audioListener = gameObject.AddComponent<AudioListener>();
                return _audioListener;
            }
        }

        /// <summary>
        /// 获取CharacterController，如果没有则添加
        /// </summary>
        public CharacterController CharacterControllerY
        {
            get
            {
                if (_characterController != null) { return _characterController; }
                _characterController = gameObject.GetComponent<CharacterController>();
                if (_characterController != null) { return _characterController; }
                _characterController = gameObject.AddComponent<CharacterController>();
                return _characterController;
            }
        }

        /// <summary>
        /// 获取Rigidbody，如果没有则添加
        /// </summary>
        public Rigidbody RigidbodyY
        {
            get
            {
                if (_rigidbody != null) { return _rigidbody; }
                _rigidbody = gameObject.GetComponent<Rigidbody>();
                if (_rigidbody != null) { return _rigidbody; }
                _rigidbody = gameObject.AddComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        /// <summary>
        /// 获取Rigidbody2D，如果没有则添加
        /// </summary>
        public Rigidbody2D Rigidbody2DY
        {
            get
            {
                if (_rigidbod2Dy != null) { return _rigidbod2Dy; }
                _rigidbod2Dy = gameObject.GetComponent<Rigidbody2D>();
                if (_rigidbod2Dy != null) { return _rigidbod2Dy; }
                _rigidbod2Dy = gameObject.AddComponent<Rigidbody2D>();
                return _rigidbod2Dy;
            }
        }

        /// <summary>
        /// 获取Canvas(UI专用)，如果没有则添加
        /// </summary>
        public Canvas CanvasY
        {
            get
            {
                if (_canvas != null) { return _canvas; }
                _canvas = gameObject.GetComponent<Canvas>();
                if (_canvas != null) { return _canvas; }
                _canvas = gameObject.AddComponent<Canvas>();
                return _canvas;
            }
        }

        /// <summary>
        /// 获取CanvasGroup(UI专用)，如果没有则添加
        /// </summary>
        public CanvasGroup CanvasGroupY
        {
            get
            {
                if (_canvasGroup != null) { return _canvasGroup; }
                _canvasGroup = gameObject.GetComponent<CanvasGroup>();
                if (_canvasGroup != null) { return _canvasGroup; }
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        /// <summary>
        /// 获取MeshFilter，如果没有则添加
        /// </summary>
        public MeshFilter MeshFilterY
        {
            get
            {
                if (_meshFilter != null) { return _meshFilter; }
                _meshFilter = gameObject.GetComponent<MeshFilter>();
                if (_meshFilter != null) { return _meshFilter; }
                _meshFilter = gameObject.AddComponent<MeshFilter>();
                return _meshFilter;
            }
        }

        /// <summary>
        /// 获取MeshRenderer，如果没有则添加
        /// </summary>
        public MeshRenderer MeshRendererY
        {
            get
            {
                if (_meshRenderer != null) { return _meshRenderer; }
                _meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (_meshRenderer != null) { return _meshRenderer; }
                _meshRenderer = gameObject.AddComponent<MeshRenderer>();
                return _meshRenderer;
            }
        }

        /// <summary>
        /// 获取Renderer，如果没有则添加
        /// </summary>
        public Renderer RendererY
        {
            get
            {
                if (_renderer != null) { return _renderer; }
                _renderer = gameObject.GetComponent<Renderer>();
                if (_renderer != null) { return _renderer; }
                _renderer = gameObject.AddComponent<Renderer>();
                return _renderer;
            }
        }

        /// <summary>
        /// 获取SkinnedMeshRenderer，如果没有则添加
        /// </summary>
        public SkinnedMeshRenderer SkinnedMeshRendererY
        {
            get
            {
                if (_skinnedMeshRenderer != null) { return _skinnedMeshRenderer; }
                _skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
                if (_skinnedMeshRenderer != null) { return _skinnedMeshRenderer; }
                _skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
                return _skinnedMeshRenderer;
            }
        }

        /// <summary>
        /// 获取SpriteRenderer，如果没有则添加
        /// </summary>
        public SpriteRenderer SpriteRendererY
        {
            get
            {
                if (_spriteRenderer != null) { return _spriteRenderer; }
                _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (_spriteRenderer != null) { return _spriteRenderer; }
                _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        /// <summary>
        /// 获取TrailRenderer，如果没有则添加
        /// </summary>
        public TrailRenderer TrailRendererY
        {
            get
            {
                if (_trailRenderer != null) { return _trailRenderer; }
                _trailRenderer = gameObject.GetComponent<TrailRenderer>();
                if (_trailRenderer != null) { return _trailRenderer; }
                _trailRenderer = gameObject.AddComponent<TrailRenderer>();
                return _trailRenderer;
            }
        }

        /// <summary>
        /// 获取LineRenderer，如果没有则添加
        /// </summary>
        public LineRenderer LineRendererY
        {
            get
            {
                if (_lineRenderer != null) { return _lineRenderer; }
                _lineRenderer = gameObject.GetComponent<LineRenderer>();
                if (_lineRenderer != null) { return _lineRenderer; }
                _lineRenderer = gameObject.AddComponent<LineRenderer>();
                return _lineRenderer;
            }
        }

        /// <summary>
        /// 获取NavMeshAgent，如果没有则添加
        /// </summary>
        public NavMeshAgent NavMeshAgentY
        {
            get
            {
                if (_navMeshAgent != null) { return _navMeshAgent; }
                _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
                if (_navMeshAgent != null) { return _navMeshAgent; }
                _navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
                _navMeshAgent.acceleration = 10000; // 加速度很大，瞬间加速
                _navMeshAgent.speed = 0; // 最大速度
                _navMeshAgent.angularSpeed = 300; // 转角速度(度/秒)
                _navMeshAgent.autoBraking = false; // 不要刹车效果(缓慢减速)
                _navMeshAgent.autoRepath = true; // 自动重新寻路，如果发现现有路径已失效，那么它将获得新的路径
                return _navMeshAgent;
            }
        }

        /// <summary>
        /// 获取NavMeshObstacle，如果没有则添加
        /// </summary>
        public NavMeshObstacle NavMeshObstacleY
        {
            get
            {
                if (_navMeshObstacle != null) { return _navMeshObstacle; }
                _navMeshObstacle = gameObject.GetComponent<NavMeshObstacle>();
                if (_navMeshObstacle != null) { return _navMeshObstacle; }
                _navMeshObstacle = gameObject.AddComponent<NavMeshObstacle>();
                return _navMeshObstacle;
            }
        }

        /// <summary>
        /// 获取BoxCollider，如果没有则添加
        /// </summary>
        public BoxCollider BoxColliderY
        {
            get
            {
                if (_boxCollider != null) { return _boxCollider; }
                _boxCollider = gameObject.GetComponent<BoxCollider>();
                if (_boxCollider != null) { return _boxCollider; }
                _boxCollider = gameObject.AddComponent<BoxCollider>();
                return _boxCollider;
            }
        }

        /// <summary>
        /// 获取BoxCollider2D，如果没有则添加
        /// </summary>
        public BoxCollider2D BoxCollider2DY
        {
            get
            {
                if (_boxCollider2D != null) { return _boxCollider2D; }
                _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
                if (_boxCollider2D != null) { return _boxCollider2D; }
                _boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
                return _boxCollider2D;
            }
        }

        /// <summary>
        /// 获取CapsuleCollider，如果没有则添加
        /// </summary>
        public CapsuleCollider CapsuleColliderY
        {
            get
            {
                if (_capsuleCollider != null) { return _capsuleCollider; }
                _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
                if (_capsuleCollider != null) { return _capsuleCollider; }
                _capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
                return _capsuleCollider;
            }
        }

        /// <summary>
        /// 获取CapsuleCollider2D，如果没有则添加
        /// </summary>
        public CapsuleCollider2D CapsuleCollider2DY
        {
            get
            {
                if (_capsuleCollider2D != null) { return _capsuleCollider2D; }
                _capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
                if (_capsuleCollider2D != null) { return _capsuleCollider2D; }
                _capsuleCollider2D = gameObject.AddComponent<CapsuleCollider2D>();
                return _capsuleCollider2D;
            }
        }

        /// <summary>
        /// 获取CircleCollider2D，如果没有则添加
        /// </summary>
        public CircleCollider2D CircleCollider2DY
        {
            get
            {
                if (_circleCollider2D != null) { return _circleCollider2D; }
                _circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
                if (_circleCollider2D != null) { return _circleCollider2D; }
                _circleCollider2D = gameObject.AddComponent<CircleCollider2D>();
                return _circleCollider2D;
            }
        }

        /// <summary>
        /// 获取EdgeCollider2D，如果没有则添加
        /// </summary>
        public EdgeCollider2D EdgeCollider2DY
        {
            get
            {
                if (_edgeCollider2D != null) { return _edgeCollider2D; }
                _edgeCollider2D = gameObject.GetComponent<EdgeCollider2D>();
                if (_edgeCollider2D != null) { return _edgeCollider2D; }
                _edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
                return _edgeCollider2D;
            }
        }

        /// <summary>
        /// 获取SpringJoint，如果没有则添加
        /// </summary>
        public SpringJoint SpringJointY
        {
            get
            {
                if (_springJoint != null) { return _springJoint; }
                _springJoint = gameObject.GetComponent<SpringJoint>();
                if (_springJoint != null) { return _springJoint; }
                _springJoint = gameObject.AddComponent<SpringJoint>();
                return _springJoint;
            }
        }

        /// <summary>
        /// 获取SpringJoint2D，如果没有则添加
        /// </summary>
        public SpringJoint2D SpringJoint2DY
        {
            get
            {
                if (_springJoint2D != null) { return _springJoint2D; }
                _springJoint2D = gameObject.GetComponent<SpringJoint2D>();
                if (_springJoint2D != null) { return _springJoint2D; }
                _springJoint2D = gameObject.AddComponent<SpringJoint2D>();
                return _springJoint2D;
            }
        }

        /// <summary>
        /// 获取TilemapCollider2D，如果没有则添加
        /// </summary>
        public TilemapCollider2D TilemapCollider2DY
        {
            get
            {
                if (_tilemapCollider2D != null) { return _tilemapCollider2D; }
                _tilemapCollider2D = gameObject.GetComponent<TilemapCollider2D>();
                if (_tilemapCollider2D != null) { return _tilemapCollider2D; }
                _tilemapCollider2D = gameObject.AddComponent<TilemapCollider2D>();
                return _tilemapCollider2D;
            }
        }

        /// <summary>
        /// 获取CompositeCollider2D，如果没有则添加
        /// </summary>
        public CompositeCollider2D CompositeCollider2DY
        {
            get
            {
                if (_compositeCollider2D != null) { return _compositeCollider2D; }
                _compositeCollider2D = gameObject.GetComponent<CompositeCollider2D>();
                if (_compositeCollider2D != null) { return _compositeCollider2D; }
                _compositeCollider2D = gameObject.AddComponent<CompositeCollider2D>();
                return _compositeCollider2D;
            }
        }
    }
    #endregion

    #region 其他缓存属性
    public partial class MonoBehaviourBaseY
    {
        private Dictionary<string, Component> _comDict;
        private Dictionary<string, Component> comDict
        {
            get
            {
                if (_comDict == null) { _comDict = new Dictionary<string, Component>(); }
                return _comDict;
            }
        }

        /// <summary>
        /// 获取缓存的组件，如果没有此组件将添加该组件并缓存起来
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ComponentOfCache<T>() where T : Component
        {
            if (comDict.TryGetValue(typeof(T).Name, out Component com))
            {
                return (T)com;
            }
            else
            {
                com = gameObject.AddComponent<T>();
                comDict.Add(typeof(T).Name, com);
                return (T)com;
            }
        }
    }
    #endregion

    #region 人体骨骼
    public partial class MonoBehaviourBaseY
    {
        /// <summary>
        /// 人体骨骼工具缓存
        /// </summary>
        public HumanBodyBoneUtil HumanBodyBoneUtil
        {
            get
            {
                if (_humanBodyBoneUtil != null) { return _humanBodyBoneUtil; }
                if (AnimatorY != null)
                {
                    _humanBodyBoneUtil = new HumanBodyBoneUtil(AnimatorY);
                }
                else
                {
                    _humanBodyBoneUtil = new HumanBodyBoneUtil();
                }
                return _humanBodyBoneUtil;
            }
        }
    }
    #endregion

    #region 在相机可见
    public partial class MonoBehaviourBaseY
    {
        /// <summary>
        /// 某个点是否在相机视野范围内
        /// </summary>
        /// <param name="camera">相机</param>
        /// <param name="worldPos">世界坐标点</param>
        /// <param name="xEdgeDistance">除去x轴边缘距离(0-0.5f)</param>
        /// <param name="yEdgeDistance">除去y轴边缘距离(0-0.5f)</param>
        /// <returns></returns>
        public bool IsVisableInCamera(Camera camera, Vector3 worldPos, float xEdgeDistance = 0, float yEdgeDistance = 0)
        {
            if (camera == null) { return false; }
            Vector3 viewPos = camera.WorldToViewportPoint(worldPos);
            if (viewPos.z < camera.nearClipPlane || viewPos.z > camera.farClipPlane) { return false; }

            float xEdge = Mathf.Clamp(xEdgeDistance, 0, 0.5f);
            float yEdge = Mathf.Clamp(yEdgeDistance, 0, 0.5f);
            // x, y 取值在 xEdge 和 yEdge 之外时代表在视角范围外
            if (viewPos.x < xEdge || viewPos.y < yEdge || viewPos.x > 1f - xEdge || viewPos.y > 1f - yEdge) { return false; }
            return true;
        }
    }
    #endregion

    #region 常用方法
    public partial class MonoBehaviourBaseY
    {
        private IEnumerator DoAfterDelayAtor(float delaySeconds, Action delayAction)
        {
            yield return new WaitForSeconds(delaySeconds);
            delayAction?.Invoke();
        }

        /// <summary>
        /// 使用协成延迟执行
        /// </summary>
        /// <param name="delaySeconds">延迟秒数</param>
        /// <param name="delayAction">延迟执行的行为</param>
        /// <returns></returns>
        public Coroutine DoAfterDelay(float delaySeconds, Action delayAction)
        {
            if (delaySeconds <= 0)
            {
                delayAction?.Invoke();
                return null;
            }
            if (!gameObject.activeInHierarchy)
            {
                return null;
            }
            return StartCoroutine(DoAfterDelayAtor(delaySeconds, delayAction));
        }
    }
    #endregion
}