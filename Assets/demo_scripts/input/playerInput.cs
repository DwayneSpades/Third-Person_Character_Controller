// GENERATED AUTOMATICALLY FROM 'Assets/demo_scripts/input/playerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""playerInput"",
    ""maps"": [
        {
            ""name"": ""playerController"",
            ""id"": ""33399018-a043-4608-9be0-1644390cf753"",
            ""actions"": [
                {
                    ""name"": ""left_stick"",
                    ""type"": ""Value"",
                    ""id"": ""05304ded-b6ac-444c-bc52-796dc57c6655"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""right_stick"",
                    ""type"": ""Value"",
                    ""id"": ""1f0142d9-0811-4f0e-9ca8-55bc92a69b1f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""snapCamFWD"",
                    ""type"": ""Button"",
                    ""id"": ""47d50777-a96e-4a2b-bec6-9241039ef7e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""jump"",
                    ""type"": ""Button"",
                    ""id"": ""db27088a-39b5-4442-b5c8-9defeda0a3a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""sprint"",
                    ""type"": ""Button"",
                    ""id"": ""76cad58e-e763-45eb-b4b6-aa20ce1e14f7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""attack"",
                    ""type"": ""Button"",
                    ""id"": ""fb23ebf5-b4c6-4da8-ab81-f6bec503d4a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""zTarget"",
                    ""type"": ""Button"",
                    ""id"": ""949e9e17-4702-4587-832e-577061b5e1a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""dash"",
                    ""type"": ""Button"",
                    ""id"": ""9d0e8281-1cec-4f52-9f32-7b055ae83325"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""mouse_look"",
                    ""type"": ""Value"",
                    ""id"": ""872705a8-0b47-416f-947c-34c557e9655b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""26a064c1-9e4f-4d07-8b18-a84fe3542f46"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""left_stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5e41bb1e-6480-4291-8742-c8fffc248bb0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""left_stick"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bd9d6825-1a93-4263-a7a1-c6abe553d4be"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""left_stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8c27a12a-26ea-4fad-9167-1fdc095d8d50"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""left_stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""90004997-5cee-489f-bbc6-a268b3928f98"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""left_stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6c1456cc-782b-431b-946b-73d4a56b0ccd"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""left_stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e9c6cc30-202c-483d-9e3a-faac7bd15249"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""right_stick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c92e4059-680d-4b92-b02e-2c9bbe10b85d"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""snapCamFWD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f83df9ea-adbf-485f-a44a-38695f09a9d0"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""snapCamFWD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3549a1a5-ff25-451d-8eef-9871a25ab001"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f65d324-8469-4673-8a51-069bad4b2fc2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0973b86-d6a3-4c69-b2e0-3173ed252f27"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1a82a1c-e91b-4f13-86c0-453e29513ce6"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""07a6c17e-8f80-44a2-b2c1-44204823438c"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ae999ec-d391-4d32-9f4d-0ca17ff85686"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d135d755-ce3d-438c-8bff-3c302e6d49cf"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""zTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""12051973-ccca-4168-a3a1-fee5680882aa"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""zTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""705fee2c-44fa-4c7e-bf0c-01afc01519f5"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5b6421a-9878-4956-b8ee-dbc4bedc81ee"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4e37b50-f93d-49c6-ad70-9b0a79e18b15"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mouse_look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // playerController
        m_playerController = asset.FindActionMap("playerController", throwIfNotFound: true);
        m_playerController_left_stick = m_playerController.FindAction("left_stick", throwIfNotFound: true);
        m_playerController_right_stick = m_playerController.FindAction("right_stick", throwIfNotFound: true);
        m_playerController_snapCamFWD = m_playerController.FindAction("snapCamFWD", throwIfNotFound: true);
        m_playerController_jump = m_playerController.FindAction("jump", throwIfNotFound: true);
        m_playerController_sprint = m_playerController.FindAction("sprint", throwIfNotFound: true);
        m_playerController_attack = m_playerController.FindAction("attack", throwIfNotFound: true);
        m_playerController_zTarget = m_playerController.FindAction("zTarget", throwIfNotFound: true);
        m_playerController_dash = m_playerController.FindAction("dash", throwIfNotFound: true);
        m_playerController_mouse_look = m_playerController.FindAction("mouse_look", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // playerController
    private readonly InputActionMap m_playerController;
    private IPlayerControllerActions m_PlayerControllerActionsCallbackInterface;
    private readonly InputAction m_playerController_left_stick;
    private readonly InputAction m_playerController_right_stick;
    private readonly InputAction m_playerController_snapCamFWD;
    private readonly InputAction m_playerController_jump;
    private readonly InputAction m_playerController_sprint;
    private readonly InputAction m_playerController_attack;
    private readonly InputAction m_playerController_zTarget;
    private readonly InputAction m_playerController_dash;
    private readonly InputAction m_playerController_mouse_look;
    public struct PlayerControllerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerControllerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @left_stick => m_Wrapper.m_playerController_left_stick;
        public InputAction @right_stick => m_Wrapper.m_playerController_right_stick;
        public InputAction @snapCamFWD => m_Wrapper.m_playerController_snapCamFWD;
        public InputAction @jump => m_Wrapper.m_playerController_jump;
        public InputAction @sprint => m_Wrapper.m_playerController_sprint;
        public InputAction @attack => m_Wrapper.m_playerController_attack;
        public InputAction @zTarget => m_Wrapper.m_playerController_zTarget;
        public InputAction @dash => m_Wrapper.m_playerController_dash;
        public InputAction @mouse_look => m_Wrapper.m_playerController_mouse_look;
        public InputActionMap Get() { return m_Wrapper.m_playerController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControllerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControllerActions instance)
        {
            if (m_Wrapper.m_PlayerControllerActionsCallbackInterface != null)
            {
                @left_stick.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnLeft_stick;
                @left_stick.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnLeft_stick;
                @left_stick.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnLeft_stick;
                @right_stick.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnRight_stick;
                @right_stick.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnRight_stick;
                @right_stick.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnRight_stick;
                @snapCamFWD.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnSnapCamFWD;
                @snapCamFWD.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnSnapCamFWD;
                @snapCamFWD.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnSnapCamFWD;
                @jump.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnJump;
                @jump.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnJump;
                @jump.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnJump;
                @sprint.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnSprint;
                @sprint.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnSprint;
                @sprint.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnSprint;
                @attack.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnAttack;
                @attack.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnAttack;
                @attack.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnAttack;
                @zTarget.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnZTarget;
                @zTarget.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnZTarget;
                @zTarget.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnZTarget;
                @dash.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnDash;
                @dash.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnDash;
                @dash.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnDash;
                @mouse_look.started -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMouse_look;
                @mouse_look.performed -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMouse_look;
                @mouse_look.canceled -= m_Wrapper.m_PlayerControllerActionsCallbackInterface.OnMouse_look;
            }
            m_Wrapper.m_PlayerControllerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @left_stick.started += instance.OnLeft_stick;
                @left_stick.performed += instance.OnLeft_stick;
                @left_stick.canceled += instance.OnLeft_stick;
                @right_stick.started += instance.OnRight_stick;
                @right_stick.performed += instance.OnRight_stick;
                @right_stick.canceled += instance.OnRight_stick;
                @snapCamFWD.started += instance.OnSnapCamFWD;
                @snapCamFWD.performed += instance.OnSnapCamFWD;
                @snapCamFWD.canceled += instance.OnSnapCamFWD;
                @jump.started += instance.OnJump;
                @jump.performed += instance.OnJump;
                @jump.canceled += instance.OnJump;
                @sprint.started += instance.OnSprint;
                @sprint.performed += instance.OnSprint;
                @sprint.canceled += instance.OnSprint;
                @attack.started += instance.OnAttack;
                @attack.performed += instance.OnAttack;
                @attack.canceled += instance.OnAttack;
                @zTarget.started += instance.OnZTarget;
                @zTarget.performed += instance.OnZTarget;
                @zTarget.canceled += instance.OnZTarget;
                @dash.started += instance.OnDash;
                @dash.performed += instance.OnDash;
                @dash.canceled += instance.OnDash;
                @mouse_look.started += instance.OnMouse_look;
                @mouse_look.performed += instance.OnMouse_look;
                @mouse_look.canceled += instance.OnMouse_look;
            }
        }
    }
    public PlayerControllerActions @playerController => new PlayerControllerActions(this);
    public interface IPlayerControllerActions
    {
        void OnLeft_stick(InputAction.CallbackContext context);
        void OnRight_stick(InputAction.CallbackContext context);
        void OnSnapCamFWD(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnZTarget(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnMouse_look(InputAction.CallbackContext context);
    }
}
