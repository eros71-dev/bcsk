using System;
using System.Collections;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	[Header("Info")]
	public string kartName = "";

	public int lap = 1;

	public int currentNode;

	public int place;

	[Header("Switches")]
	public bool steerInAir = true;

	private bool motorcycleTilt;

	private bool alwaysSmoke;

	public bool controllable = true;

	public bool autoPilot;

	public bool isPlayer;

	public bool isFrozen;

	public bool raceOver;

	[Header("Components")]
	public VehiclePrefabs vehiclePrefabs;

	private GameObject currentVehicleModel;

	public ItemPrefabs itemPrefabs;

	public HeldItemPrefabs heldItemPrefabs;

	public EffectsPrefabs effectsPrefabs;

	public Rigidbody sphere;

	public Transform ModelBaseContainer;

	public NearbyDetector nearbyDetector;

	public Transform WiggleHurtContainer;

	private Vector3 WiggleHurtContainerStartPos;

	public TrailRenderer trailLeft;

	public TrailRenderer trailRight;

	public ParticleSystem smoke;

	public Transform itemThrowPosFront;

	public Transform itemThrowPosBack;

	public GameObject hitCam;

	public GameObject hitSmokePrefab;

	public RacerInfo racerInfo;

	public UIManager uIManager;

	public Path path;

	public PlaceTracker placeTracker;

	[Header("Controls")]
	public KeyCode accelerate;

	public KeyCode brake;

	public KeyCode steerLeft;

	public KeyCode steerRight;

	public KeyCode throwItem;

	[Header("Parameters")]
	[Range(5f, 80f)]
	public float topSpeed = 30f;

	[Range(1f, 80f)]
	public float acceleration = 4f;

	[Range(20f, 160f)]
	public float steering = 80f;

	[Range(0f, 50f)]
	public float gravity = 10f;

	[Range(0f, 1f)]
	public float drift = 1f;

	private float speed;

	private float speedTarget;

	private float rotate;

	private float rotateTarget;

	public bool nearGround;

	public bool onGround;

	private bool hitPre;

	private bool hitMid;

	private bool hitPost;

	private bool hurt;

	private float currHurtTime;

	private float maxHurtTime = 1.5f;

	private bool isBoosting;

	private float currBoostTime;

	private float maxBoostTime = 0.8f;

	private float normalFOV = 60f;

	private float boostFOV = 76f;

	private float currFOV = 60f;

	private float currFOVTime;

	private float targetFOV = 60f;

	private float lastTargetFOV = 60f;

	private float normalTopSpeed;

	private float normalAcceleration;

	private float boostTopSpeed;

	private float boostAcceleration;

	private ItemEnum.item currentItem;

	private float currAutoPilotItemTime;

	private float maxAutoPilotItemTime = 3f;

	private GameObject currentHeldItem;

	private float autoPilotRandomTurnTimeCurr;

	private float lastDistance = 999f;

	public float wrongWayTime;

	private bool isGoingWrongWay;

	[Header("Sounds")]
	public AudioSource throwItemSound;

	public AudioSource hitSound;

	public AudioSource getItemSound;

	public AudioSource engineIdleSound;

	public AudioSource engineLoopSound;

	public AudioSource engineSpeedUpSound;

	public AudioSource engineSlowDownSound;

	public AudioSource tireScreechSound;

	public AudioSource thumpGroundSound;

	public AudioSource boostSound;

	private bool isBraking;

	private bool isReverseing;

	public int currClosestNode = -1;

	public bool rubberBanding;

	public bool superRubberBanding;

	private float timeInFirstPlace;

	private float autoPilotRandomSpeedOffset = 2f;

	private float autoPilotRandomAccelerationOffset = 3.5f;

	private void Awake()
	{
		WiggleHurtContainerStartPos = WiggleHurtContainer.localPosition;
	}

	private void Start()
	{
		normalTopSpeed = topSpeed;
		normalAcceleration = acceleration;
		boostTopSpeed = normalTopSpeed * 2f;
		boostAcceleration = normalAcceleration * 2f;
		ParticleSystem.EmissionModule emission = smoke.emission;
		emission.enabled = false;
		if (isPlayer && !raceOver && (bool)uIManager && isPlayer)
		{
			uIManager.lookoutImage.gameObject.SetActive(value: false);
		}
		if (isPlayer)
		{
			throwItemSound.spatialBlend = 0f;
			hitSound.spatialBlend = 0f;
			getItemSound.spatialBlend = 0f;
		}
		if (autoPilot)
		{
			ShuffleStats();
			autoPilotRandomTurnTimeCurr = UnityEngine.Random.Range(1, 8);
		}
		if ((bool)engineIdleSound)
		{
			engineIdleSound.Play();
			engineIdleSound.volume = 1f;
		}
		if ((bool)engineLoopSound)
		{
			engineLoopSound.Play();
			engineLoopSound.volume = 0f;
		}
	}

	private void Update()
	{
		if (isFrozen || GlobalGameData.isPaused)
		{
			return;
		}
		ManageBoost();
		ManageHurt();
		ManageLookOut();
		if (!hurt)
		{
			float from = speedTarget;
			_ = autoPilot;
			if (autoPilot && Vector3.Distance(base.transform.position, path.nodes[currentNode].position) < 26f)
			{
				from = speedTarget / 1.01f;
			}
			speedTarget = Mathf.SmoothStep(from, speed, Time.deltaTime * acceleration);
			speed = 0f;
			float value = Mathf.Abs(speedTarget);
			value = Remap(value, 0f, 29f, 0f, 1.1f);
			if ((bool)engineLoopSound)
			{
				engineLoopSound.volume = value;
				engineLoopSound.pitch = 1f + value / 2f;
			}
			if ((bool)engineIdleSound)
			{
				engineIdleSound.volume = 1f - value;
			}
			if (isPlayer)
			{
				CheckIfGoingBackwards();
			}
			if (isPlayer && currClosestNode > currentNode && Vector3.Distance(base.transform.position, path.nodes[currClosestNode].position) < 25f && !isGoingWrongWay)
			{
				currentNode = currClosestNode;
			}
			if (Vector3.Distance(base.transform.position, path.nodes[currentNode].position) < 25f)
			{
				if (currentNode == path.nodes.Count - 1)
				{
					currentNode = 0;
				}
				else
				{
					currentNode++;
				}
			}
			if (autoPilot)
			{
				ControlAccelerate();
				if (currentItem != 0)
				{
					currAutoPilotItemTime += Time.deltaTime;
					if (currAutoPilotItemTime >= maxAutoPilotItemTime)
					{
						currAutoPilotItemTime = 0f;
						maxAutoPilotItemTime = UnityEngine.Random.Range(3, 6);
						ThrowItem(1);
					}
				}
			}
			else
			{
				if (Input.GetKey(accelerate))
				{
					ControlAccelerate();
				}
				if (Input.GetKey(KeyCode.W))
				{
					ControlAccelerate();
				}
				if (Input.GetKey(brake) || Input.GetKey(KeyCode.S))
				{
					ControlBrake();
				}
				else
				{
					isBraking = false;
				}
				if (Input.GetKeyDown(throwItem))
				{
					ThrowItem(1);
				}
				if (Input.GetKeyDown(KeyCode.LeftControl))
				{
					ThrowItem(0);
				}
			}
			rotateTarget = Mathf.Lerp(rotateTarget, rotate, Time.deltaTime * 4f);
			rotate = 0f;
			if (autoPilot)
			{
				Vector3 vector = base.transform.InverseTransformPoint(path.nodes[currentNode].position);
				float direction = vector.x / vector.magnitude;
				if (Vector3.Distance(base.transform.position, path.nodes[currentNode].position) > 20f)
				{
					autoPilotRandomTurnTimeCurr -= Time.deltaTime;
					if (autoPilotRandomTurnTimeCurr <= 0f && !rubberBanding && !superRubberBanding)
					{
						if (autoPilotRandomTurnTimeCurr <= -0.5f)
						{
							autoPilotRandomTurnTimeCurr = UnityEngine.Random.Range(1, 8);
						}
						ControlSteer(UnityEngine.Random.Range(-1, 1));
					}
					else
					{
						ControlSteer(direction);
					}
				}
				else
				{
					ControlSteer(direction);
				}
			}
			else
			{
				if (Input.GetKey(steerLeft))
				{
					ControlSteer(-1f);
				}
				if (Input.GetKey(KeyCode.A))
				{
					ControlSteer(-1f);
				}
				if (Input.GetKey(steerRight))
				{
					ControlSteer(1f);
				}
				if (Input.GetKey(KeyCode.D))
				{
					ControlSteer(1f);
				}
			}
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(new Vector3(0f, base.transform.eulerAngles.y + rotateTarget, 0f)), Time.deltaTime * 2f);
			currentVehicleModel.GetComponent<VehicleModel>().wheelFrontLeftHolder.localRotation = Quaternion.Euler(0f, rotateTarget / 2f, 0f);
			currentVehicleModel.GetComponent<VehicleModel>().wheelFrontRightHolder.localRotation = Quaternion.Euler(0f, rotateTarget / 2f, 0f);
			currentVehicleModel.GetComponent<VehicleModel>().wheelFrontRight.Rotate(new Vector3(speedTarget * 40f, 0f, 0f) * Time.deltaTime);
			currentVehicleModel.GetComponent<VehicleModel>().wheelFrontLeft.Rotate(new Vector3(speedTarget * 40f, 0f, 0f) * Time.deltaTime);
			currentVehicleModel.GetComponent<VehicleModel>().wheelRearLeft.Rotate(new Vector3(speedTarget * 40f, 0f, 0f) * Time.deltaTime);
			currentVehicleModel.GetComponent<VehicleModel>().wheelRearRight.Rotate(new Vector3(speedTarget * 40f, 0f, 0f) * Time.deltaTime);
			currentVehicleModel.GetComponent<VehicleModel>().vehicleBody.localRotation = Quaternion.Slerp(currentVehicleModel.GetComponent<VehicleModel>().vehicleBody.localRotation, Quaternion.Euler(new Vector3(0f, 0f, rotateTarget / -4f)), Time.deltaTime * 4f);
			if (!motorcycleTilt)
			{
				smoke.transform.localPosition = new Vector3((0f - rotateTarget) / 100f, smoke.transform.localPosition.y, smoke.transform.localPosition.z);
			}
			bool flag = false;
			flag = (onGround && sphere.velocity.magnitude > topSpeed / 4f && (Vector3.Angle(sphere.velocity, ModelBaseContainer.forward) > 40f || alwaysSmoke) && !isReverseing) || isBraking;
			if (sphere.velocity.y < -5f && onGround && !thumpGroundSound.isPlaying)
			{
				thumpGroundSound.Play();
			}
			ParticleSystem.EmissionModule emission = smoke.emission;
			emission.enabled = flag;
			if (trailLeft != null)
			{
				trailLeft.emitting = smoke.emission.enabled;
			}
			if (trailRight != null)
			{
				trailRight.emitting = smoke.emission.enabled;
			}
			if (smoke.emission.enabled)
			{
				if ((bool)tireScreechSound && !tireScreechSound.isPlaying)
				{
					tireScreechSound.Play();
				}
			}
			else
			{
				tireScreechSound.Stop();
			}
			if (speed == 0f && sphere.velocity.magnitude < 4f)
			{
				sphere.velocity = Vector3.Lerp(sphere.velocity, Vector3.zero, Time.deltaTime * 2f);
			}
		}
		else
		{
			currentVehicleModel.GetComponent<VehicleModel>().vehicleBody.localRotation = Quaternion.Slerp(currentVehicleModel.GetComponent<VehicleModel>().vehicleBody.localRotation, Quaternion.identity, Time.deltaTime * 4f);
		}
		HandleAntiRubberBand();
		if (base.transform.position.y < -150f)
		{
			Debug.Log("fell through floor");
			sphere.transform.position = path.nodes[currClosestNode].position + Vector3.up * 1.5f;
			base.transform.position = path.nodes[currClosestNode].position + Vector3.up * 1.5f;
		}
	}

	private void FixedUpdate()
	{
		_ = autoPilot;
		onGround = Physics.Raycast(base.transform.position, Vector3.down, out var _, 1.5f);
		nearGround = Physics.Raycast(base.transform.position, Vector3.down, out var hitInfo2, 2f);
		ModelBaseContainer.up = Vector3.Lerp(ModelBaseContainer.up, hitInfo2.normal, Time.deltaTime * 8f);
		ModelBaseContainer.Rotate(0f, base.transform.eulerAngles.y, 0f);
		if (nearGround)
		{
			sphere.AddForce(ModelBaseContainer.forward * speedTarget, ForceMode.Acceleration);
		}
		else
		{
			sphere.AddForce(ModelBaseContainer.forward * (speedTarget / 10f), ForceMode.Acceleration);
			sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
		}
		base.transform.position = sphere.transform.position + new Vector3(0f, 0.35f, 0f);
		Vector3 vector = base.transform.InverseTransformVector(sphere.velocity);
		vector.x *= 0.9f + drift / 10f;
		if (nearGround)
		{
			sphere.velocity = base.transform.TransformVector(vector);
		}
	}

	public void ControlAccelerate()
	{
		Debug.Log("Accelerate");
		if (controllable)
		{
			speed = topSpeed;
			isBraking = false;
			isReverseing = false;
		}
	}

	public void ControlBrake()
	{
		if (controllable)
		{
			if (speedTarget <= 0f)
			{
				speed = (0f - topSpeed) / 2f;
				isBraking = false;
				isReverseing = true;
			}
			else
			{
				speed = 0f - topSpeed;
				isBraking = true;
				isReverseing = false;
			}
		}
	}

	public void ControlSteer(float direction)
	{
		if (controllable && (nearGround || steerInAir))
		{
			rotate = steering * direction;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.GetComponent<PhysicsObject>())
		{
			other.GetComponent<PhysicsObject>().Hit(sphere.velocity);
		}
		if (other.CompareTag("FinishLine"))
		{
			if (!raceOver && hitPost && hitMid && hitPre)
			{
				Debug.Log("FINISH");
				currentNode = 0;
				if (lap == path.laps)
				{
					if (!raceOver)
					{
						lap++;
						raceOver = true;
						autoPilot = true;
						steering = 90f;
						VehicleCamera component;
						if ((bool)(component = GetComponent<VehicleCamera>()))
						{
							component.spin = true;
						}
						if ((bool)uIManager && isPlayer)
						{
							uIManager.itemText.enabled = false;
							uIManager.lapText.enabled = false;
						}
						if ((bool)currentVehicleModel.GetComponent<VehicleModel>().driver)
						{
							if (place == 1 || place == 2 || place == 3)
							{
								currentVehicleModel.GetComponent<VehicleModel>().driver.Cheer();
							}
							else
							{
								currentVehicleModel.GetComponent<VehicleModel>().driver.Sad();
							}
						}
						if (isPlayer)
						{
							placeTracker.FinishRace();
						}
					}
				}
				else
				{
					lap++;
					hitPost = false;
					hitMid = false;
					hitPre = false;
					if (isPlayer)
					{
						if (lap == path.laps)
						{
							placeTracker.FinalLap();
						}
						else
						{
							placeTracker.NextLap();
						}
					}
					else if (lap == path.laps)
					{
						Debug.Log(racerInfo.racerName + "is on final lap");
						if (GlobalGameData.numTimesLost[GlobalGameData.currentCourse] > 0)
						{
							Debug.Log("Player Lost this race once so slow down");
							SetShittyStats();
						}
					}
				}
			}
		}
		else if (other.CompareTag("PreFinishLine"))
		{
			if (hitPost && hitMid)
			{
				hitPre = true;
			}
		}
		else if (other.CompareTag("MidPoint"))
		{
			if (hitPost)
			{
				hitMid = true;
			}
		}
		else if (other.CompareTag("PostFinishLine"))
		{
			hitPost = true;
		}
		if (other.gameObject.CompareTag("ItemBox"))
		{
			other.GetComponent<ItemBox>().Hide();
			GetItem();
		}
	}

	public void SetPosition(Vector3 position, Quaternion rotation)
	{
		speed = (rotate = 0f);
		sphere.velocity = Vector3.zero;
		sphere.position = position;
		base.transform.position = position;
		base.transform.rotation = rotation;
	}

	public void TakeDammage(Item itemDealingDammage)
	{
		if ((bool)itemDealingDammage && itemDealingDammage.throwBy.isPlayer)
		{
			placeTracker.VehicleHitByPlayer(this);
		}
		UnityEngine.Object.Instantiate(hitSmokePrefab, base.transform.position, Quaternion.identity, base.transform);
		speedTarget = 0f;
		hurt = true;
		currHurtTime = 0f;
		ParticleSystem.EmissionModule emission = smoke.emission;
		emission.enabled = true;
		if ((bool)currentVehicleModel.GetComponent<VehicleModel>().driver)
		{
			currentVehicleModel.GetComponent<VehicleModel>().driver.Hurt();
		}
		if ((bool)hitSound)
		{
			hitSound.Play();
		}
		if ((bool)currentHeldItem)
		{
			UnityEngine.Object.Destroy(currentHeldItem);
			currentHeldItem = null;
		}
		Driver driver = currentVehicleModel.GetComponent<VehicleModel>().driver;
		if ((bool)driver)
		{
			driver.StopHoldItem();
		}
		currentItem = ItemEnum.item.none;
		if (isPlayer)
		{
			uIManager.itemText.text = "Item: None";
		}
		engineLoopSound.volume = 0f;
		engineLoopSound.pitch = 1f;
		engineIdleSound.volume = 1f;
		engineSlowDownSound.Play();
	}

	public void GetItem()
	{
		if (currentItem != 0 || raceOver)
		{
			return;
		}
		Array values = Enum.GetValues(typeof(ItemEnum.item));
		currentItem = (ItemEnum.item)values.GetValue(UnityEngine.Random.Range(1, values.Length));
		if (place == 1)
		{
			float[] weights = new float[6] { 0f, 1f, 1f, 0f, 0f, 0f };
			int randomWeightedIndex = GetRandomWeightedIndex(weights);
			currentItem = (ItemEnum.item)randomWeightedIndex;
		}
		else if (place == 6)
		{
			float[] weights2 = new float[6] { 0f, 0f, 0f, 10f, 2f, 11f };
			int randomWeightedIndex2 = GetRandomWeightedIndex(weights2);
			currentItem = (ItemEnum.item)randomWeightedIndex2;
		}
		else if (place >= 4)
		{
			float[] weights3 = new float[6] { 0f, 0f, 0f, 3f, 1f, 3f };
			int randomWeightedIndex3 = GetRandomWeightedIndex(weights3);
			currentItem = (ItemEnum.item)randomWeightedIndex3;
		}
		else
		{
			float[] weights4 = new float[6] { 0f, 1f, 1f, 5f, 4f, 5f };
			int randomWeightedIndex4 = GetRandomWeightedIndex(weights4);
			currentItem = (ItemEnum.item)randomWeightedIndex4;
		}
		Driver driver = currentVehicleModel.GetComponent<VehicleModel>().driver;
		if ((bool)driver)
		{
			driver.StartHoldItem();
		}
		if ((bool)getItemSound)
		{
			getItemSound.Play();
		}
		switch (currentItem)
		{
		case ItemEnum.item.banana:
			if ((bool)uIManager && isPlayer)
			{
				uIManager.itemText.text = "Item: Banana";
			}
			if ((bool)driver)
			{
				currentHeldItem = UnityEngine.Object.Instantiate(heldItemPrefabs.bananaPrefab, driver.itemHoldLocation);
			}
			break;
		case ItemEnum.item.redShell:
			if ((bool)uIManager && isPlayer)
			{
				uIManager.itemText.text = "Item: Red Shell";
			}
			if ((bool)driver)
			{
				currentHeldItem = UnityEngine.Object.Instantiate(heldItemPrefabs.redShellPrefab, driver.itemHoldLocation);
			}
			break;
		case ItemEnum.item.greenShell:
			if ((bool)uIManager && isPlayer)
			{
				uIManager.itemText.text = "Item: Green Shell";
			}
			if ((bool)driver)
			{
				currentHeldItem = UnityEngine.Object.Instantiate(heldItemPrefabs.greenShellPrefab, driver.itemHoldLocation);
			}
			break;
		case ItemEnum.item.bomb:
			if ((bool)uIManager && isPlayer)
			{
				uIManager.itemText.text = "Item: Bomb";
			}
			if ((bool)driver)
			{
				currentHeldItem = UnityEngine.Object.Instantiate(heldItemPrefabs.bombPrefab, driver.itemHoldLocation);
			}
			break;
		case ItemEnum.item.boost:
			if ((bool)uIManager && isPlayer)
			{
				uIManager.itemText.text = "Item: Mushroom";
			}
			if ((bool)driver)
			{
				currentHeldItem = UnityEngine.Object.Instantiate(heldItemPrefabs.boostPrefab, driver.itemHoldLocation);
			}
			break;
		}
	}

	public void ThrowItem(int direction)
	{
		if (currentItem == ItemEnum.item.none)
		{
			Debug.Log("NO ITEM TO THROW");
			return;
		}
		if ((bool)throwItemSound)
		{
			throwItemSound.Play();
		}
		GameObject gameObject = null;
		if ((bool)currentHeldItem)
		{
			UnityEngine.Object.Destroy(currentHeldItem);
			currentHeldItem = null;
		}
		Driver driver = currentVehicleModel.GetComponent<VehicleModel>().driver;
		if ((bool)driver)
		{
			driver.StopHoldItem();
		}
		float num = speedTarget + 1f;
		if (direction == 0 && isPlayer)
		{
			switch (currentItem)
			{
			case ItemEnum.item.banana:
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.bananaPrefab, itemThrowPosBack.position, Quaternion.identity);
				gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 100f, 0f) + base.transform.forward * -100f);
				break;
			case ItemEnum.item.redShell:
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.redShellPrefab, itemThrowPosBack.position, Quaternion.identity);
				placeTracker.getVehicleAheadOfMe(place);
				gameObject.transform.eulerAngles = base.transform.eulerAngles - new Vector3(0f, 180f, 0f);
				break;
			case ItemEnum.item.greenShell:
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.greenShellPrefab, itemThrowPosBack.position, Quaternion.identity);
				gameObject.GetComponent<Rigidbody>().velocity = -base.transform.forward * (25f + num / 2f);
				break;
			case ItemEnum.item.bomb:
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.bombPrefab, itemThrowPosBack.position, Quaternion.identity);
				gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 150f, 0f) + base.transform.forward * -150f);
				break;
			case ItemEnum.item.boost:
				startBoost();
				break;
			}
		}
		else
		{
			switch (currentItem)
			{
			case ItemEnum.item.banana:
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.bananaPrefab, itemThrowPosFront.position, Quaternion.identity);
				gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 350f, 0f) + base.transform.forward * (900f + num * 22f));
				break;
			case ItemEnum.item.redShell:
			{
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.redShellPrefab, itemThrowPosFront.position, Quaternion.identity);
				GameObject vehicleAheadOfMe = placeTracker.getVehicleAheadOfMe(place);
				gameObject.GetComponent<RedShell2>().target = vehicleAheadOfMe;
				gameObject.transform.eulerAngles = base.transform.eulerAngles;
				break;
			}
			case ItemEnum.item.greenShell:
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.greenShellPrefab, itemThrowPosFront.position, Quaternion.identity);
				gameObject.GetComponent<Rigidbody>().velocity = base.transform.forward * (25f + num / 2f);
				break;
			case ItemEnum.item.bomb:
				gameObject = UnityEngine.Object.Instantiate(itemPrefabs.bombPrefab, itemThrowPosFront.position, Quaternion.identity);
				gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 450f, 0f) + base.transform.forward * (1000f + num * 22f));
				break;
			case ItemEnum.item.boost:
				startBoost();
				break;
			}
		}
		if ((bool)gameObject && gameObject.TryGetComponent<Item>(out var component))
		{
			component.throwBy = this;
		}
		currentItem = ItemEnum.item.none;
		if (!raceOver && isPlayer && (bool)uIManager && isPlayer)
		{
			uIManager.itemText.text = "Item: None";
		}
		if ((bool)currentVehicleModel.GetComponent<VehicleModel>().driver)
		{
			currentVehicleModel.GetComponent<VehicleModel>().driver.ThrowItem();
		}
	}

	private void startBoost()
	{
		if (!isBoosting)
		{
			currBoostTime = 0f;
			currFOVTime = 0f;
			if (isPlayer)
			{
				boostSound.spatialBlend = 0f;
			}
			boostSound.Play();
		}
		isBoosting = true;
		topSpeed = boostTopSpeed;
		acceleration = boostAcceleration;
		speedTarget = topSpeed;
		lastTargetFOV = currFOV;
		targetFOV = boostFOV;
	}

	private void stopBoost()
	{
		currBoostTime = 0f;
		isBoosting = false;
		currFOVTime = 0f;
		topSpeed = normalTopSpeed;
		acceleration = normalAcceleration;
		lastTargetFOV = currFOV;
		targetFOV = normalFOV;
	}

	private void ManageBoost()
	{
		if (isBoosting)
		{
			currBoostTime += Time.deltaTime;
			if (currBoostTime >= maxBoostTime)
			{
				stopBoost();
			}
		}
		if (isBoosting)
		{
			currFOVTime += Time.deltaTime * 6f;
		}
		else
		{
			currFOVTime += Time.deltaTime * 1f;
		}
		currFOV = Mathf.Lerp(lastTargetFOV, targetFOV, currFOVTime);
		VehicleCamera component;
		if ((bool)(component = GetComponent<VehicleCamera>()))
		{
			component.vehicleCamera.fieldOfView = currFOV;
		}
	}

	private void ManageHurt()
	{
		if (hurt)
		{
			float num = 15f;
			float y = 21f * Mathf.Sin(Time.timeSinceLevelLoad * num);
			WiggleHurtContainer.localRotation = Quaternion.Slerp(WiggleHurtContainer.localRotation, Quaternion.Euler(0f, y, 0f), Time.deltaTime * 10f);
			currHurtTime += Time.deltaTime;
			if (currHurtTime >= maxHurtTime)
			{
				currHurtTime = 0f;
				hurt = false;
				ParticleSystem.EmissionModule emission = smoke.emission;
				emission.enabled = false;
				if ((bool)currentVehicleModel.GetComponent<VehicleModel>().driver)
				{
					currentVehicleModel.GetComponent<VehicleModel>().driver.Idle();
				}
			}
		}
		else
		{
			WiggleHurtContainer.localRotation = Quaternion.identity;
		}
	}

	private void ManageLookOut()
	{
		if (!isPlayer)
		{
			return;
		}
		bool flag = false;
		foreach (GameObject nearbyHazard in nearbyDetector.nearbyHazards)
		{
			if ((bool)nearbyHazard && !nearbyHazard.GetComponent<Explosion>())
			{
				Item component = nearbyHazard.GetComponent<Item>();
				if (component != null && component.GetItemIsActive() && Vector3.Distance(base.transform.position, nearbyHazard.transform.position) >= 1f)
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			if ((bool)uIManager && isPlayer)
			{
				uIManager.lookoutImage.gameObject.SetActive(value: true);
			}
		}
		else if ((bool)uIManager && isPlayer)
		{
			uIManager.lookoutImage.gameObject.SetActive(value: false);
		}
	}

	private void CheckIfGoingBackwards()
	{
		float num = 100000000f;
		int num2 = 0;
		foreach (Transform node in path.nodes)
		{
			float num3 = Vector3.Distance(base.transform.position, node.position);
			if (num3 < num)
			{
				num = num3;
				currClosestNode = num2;
			}
			num2++;
		}
		int num4 = -1;
		num4 = ((currClosestNode != path.nodes.Count - 1) ? (currClosestNode + 1) : 0);
		if ((double)Vector3.Dot((base.transform.position - path.nodes[num4].transform.position).normalized, base.transform.forward) > 0.6)
		{
			if (wrongWayTime >= 0.75f)
			{
				wrongWayTime = 1.5f;
				Debug.Log("WRONG WAY!");
				uIManager.wrongWayText.gameObject.SetActive(value: true);
				isGoingWrongWay = true;
			}
			else
			{
				wrongWayTime += Time.deltaTime;
			}
		}
		else if (wrongWayTime > 0f)
		{
			wrongWayTime -= Time.deltaTime;
		}
		else if (wrongWayTime <= 0f)
		{
			wrongWayTime = 0f;
			uIManager.wrongWayText.gameObject.SetActive(value: false);
			isGoingWrongWay = false;
		}
	}

	public bool GetHitMid()
	{
		return hitMid;
	}

	private float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	private IEnumerator StopSoundAfterXTime(AudioSource src, float time)
	{
		yield return new WaitForSeconds(time);
		src.Stop();
	}

	public void StartRubberBanding()
	{
		if (!rubberBanding && !isPlayer && lap != path.laps)
		{
			rubberBanding = true;
			topSpeed = 38f;
			acceleration = 39f;
			steering = 150f;
			drift = 0.1f;
			normalTopSpeed = topSpeed;
			normalAcceleration = acceleration;
			boostTopSpeed = normalTopSpeed;
			boostAcceleration = normalAcceleration;
		}
	}

	public void StartSuperRubberBanding()
	{
		if (!superRubberBanding && !isPlayer && lap != path.laps)
		{
			superRubberBanding = true;
			topSpeed = 70f;
			acceleration = 70f;
			steering = 150f;
			drift = 0.1f;
			normalTopSpeed = topSpeed;
			normalAcceleration = acceleration;
			boostTopSpeed = normalTopSpeed;
			boostAcceleration = normalAcceleration;
		}
	}

	public void StopRubberBanding()
	{
		if ((rubberBanding || superRubberBanding) && !isPlayer)
		{
			rubberBanding = false;
			superRubberBanding = false;
			ShuffleStats();
			if (GlobalGameData.numTimesLost[GlobalGameData.currentCourse] > 0)
			{
				Debug.Log("Player Lost this race once so slow down after StopRubberBanding");
				SetShittyStats();
			}
		}
	}

	public void SetRacer(int prefabNum)
	{
		currentVehicleModel = UnityEngine.Object.Instantiate(vehiclePrefabs.prefabs[prefabNum], WiggleHurtContainer);
		racerInfo = currentVehicleModel.GetComponent<RacerInfo>();
		kartName = racerInfo.racerName;
	}

	private void HandleAntiRubberBand()
	{
		if (place == 1 && !isPlayer)
		{
			timeInFirstPlace += Time.deltaTime;
			if (timeInFirstPlace > 30f)
			{
				Debug.Log("Slow Down: " + racerInfo.racerName);
				SetBaseStats();
				topSpeed -= 0.5f;
				normalTopSpeed = topSpeed;
				normalAcceleration = acceleration;
				boostTopSpeed = normalTopSpeed * 2f;
				boostAcceleration = normalAcceleration * 2f;
				timeInFirstPlace = 0f;
			}
		}
	}

	public void ShuffleStats()
	{
		SetBaseStats();
		topSpeed = UnityEngine.Random.Range(topSpeed, topSpeed + autoPilotRandomSpeedOffset);
		acceleration = UnityEngine.Random.Range(acceleration, acceleration + autoPilotRandomAccelerationOffset);
		normalTopSpeed = topSpeed;
		normalAcceleration = acceleration;
		boostTopSpeed = normalTopSpeed * 2f;
		boostAcceleration = normalAcceleration * 2f;
	}

	private void SetBaseStats()
	{
		topSpeed = 30f;
		acceleration = 15f;
		steering = 53f;
		drift = 0.9f;
		if (GlobalGameData.currentCourse == 1)
		{
			topSpeed += 0.7f;
			acceleration += 0.74f;
		}
		else if (GlobalGameData.currentCourse == 2)
		{
			topSpeed -= 0.2f;
			acceleration -= 0f;
		}
		else if (GlobalGameData.currentCourse == 3)
		{
			topSpeed -= 0.8f;
			acceleration -= 0.2f;
			steering -= 1f;
		}
	}

	public Driver GetDriver()
	{
		return currentVehicleModel.GetComponent<VehicleModel>().driver;
	}

	public VehicleModel GetVehicleModel()
	{
		return currentVehicleModel.GetComponent<VehicleModel>();
	}

	public void PauseAllSound()
	{
		engineIdleSound.Pause();
		engineLoopSound.Pause();
		tireScreechSound.Pause();
	}

	public void ResumeAllSound()
	{
		engineIdleSound.Play();
		engineLoopSound.Play();
		tireScreechSound.Play();
	}

	public int GetRandomWeightedIndex(float[] weights)
	{
		if (weights == null || weights.Length == 0)
		{
			return -1;
		}
		float num = 0f;
		for (int i = 0; i < weights.Length; i++)
		{
			float num2 = weights[i];
			if (float.IsPositiveInfinity(num2))
			{
				return i;
			}
			if (num2 >= 0f && !float.IsNaN(num2))
			{
				num += weights[i];
			}
		}
		float value = UnityEngine.Random.value;
		float num3 = 0f;
		for (int i = 0; i < weights.Length; i++)
		{
			float num2 = weights[i];
			if (!float.IsNaN(num2) && !(num2 <= 0f))
			{
				num3 += num2 / num;
				if (num3 >= value)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public void SetShittyStats()
	{
		topSpeed = 30f - (float)GlobalGameData.numTimesLost[GlobalGameData.currentCourse];
		acceleration = 15f - (float)GlobalGameData.numTimesLost[GlobalGameData.currentCourse];
		if (topSpeed < 10f)
		{
			topSpeed = 10f;
		}
		if (acceleration < 5f)
		{
			acceleration = 5f;
		}
		normalTopSpeed = topSpeed;
		normalAcceleration = acceleration;
		boostTopSpeed = normalTopSpeed * 2f;
		boostAcceleration = normalAcceleration * 2f;
	}
}
