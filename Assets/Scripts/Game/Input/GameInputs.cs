// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Game/Input/GameInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @GameInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputs"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""511fc95e-674a-45c0-8a95-4bae557b61f2"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""cd325d1c-1e57-4d22-ad47-1b14f72d428d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Magnet"",
                    ""type"": ""Button"",
                    ""id"": ""a0d61f42-875b-4c45-9f79-285655cafe40"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Thruster"",
                    ""type"": ""Button"",
                    ""id"": ""96f0b111-c01b-4bd6-8b18-4e99e27bb9fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Precise Input"",
                    ""type"": ""Value"",
                    ""id"": ""e0b28939-741e-4a49-a780-42411e1e872e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""21a86d8f-b12a-40aa-9e26-4868eb24595d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81cc2be3-1219-4854-8c61-f86146264c9a"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Magnet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6965eb7-739f-4274-9b2d-92ccc2c38535"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Thruster"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""cafea08b-3bb9-440c-b940-7ab0cf394375"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Precise Input"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4e5f1093-c1e7-47fb-881b-d0ed70d230da"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Precise Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""da2411f7-b526-4704-947b-76d753fb77ac"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Precise Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5359ae7f-884a-4f2a-b35b-69cea44e52de"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Precise Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a2d0b475-9482-41f0-832b-215835e2ec5f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Precise Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_Magnet = m_Player.FindAction("Magnet", throwIfNotFound: true);
        m_Player_Thruster = m_Player.FindAction("Thruster", throwIfNotFound: true);
        m_Player_PreciseInput = m_Player.FindAction("Precise Input", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_Magnet;
    private readonly InputAction m_Player_Thruster;
    private readonly InputAction m_Player_PreciseInput;
    public struct PlayerActions
    {
        private @GameInputs m_Wrapper;
        public PlayerActions(@GameInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @Magnet => m_Wrapper.m_Player_Magnet;
        public InputAction @Thruster => m_Wrapper.m_Player_Thruster;
        public InputAction @PreciseInput => m_Wrapper.m_Player_PreciseInput;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Magnet.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMagnet;
                @Magnet.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMagnet;
                @Magnet.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMagnet;
                @Thruster.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThruster;
                @Thruster.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThruster;
                @Thruster.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnThruster;
                @PreciseInput.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPreciseInput;
                @PreciseInput.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPreciseInput;
                @PreciseInput.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPreciseInput;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Magnet.started += instance.OnMagnet;
                @Magnet.performed += instance.OnMagnet;
                @Magnet.canceled += instance.OnMagnet;
                @Thruster.started += instance.OnThruster;
                @Thruster.performed += instance.OnThruster;
                @Thruster.canceled += instance.OnThruster;
                @PreciseInput.started += instance.OnPreciseInput;
                @PreciseInput.performed += instance.OnPreciseInput;
                @PreciseInput.canceled += instance.OnPreciseInput;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnMagnet(InputAction.CallbackContext context);
        void OnThruster(InputAction.CallbackContext context);
        void OnPreciseInput(InputAction.CallbackContext context);
    }
}
