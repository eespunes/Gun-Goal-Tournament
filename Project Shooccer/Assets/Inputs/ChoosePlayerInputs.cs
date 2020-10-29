// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/ChoosePlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ChoosePlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ChoosePlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ChoosePlayerInputs"",
    ""maps"": [
        {
            ""name"": ""gameplay"",
            ""id"": ""265c38f5-dd18-4d34-b198-aec58e1627ff"",
            ""actions"": [
                {
                    ""name"": ""previous kit"",
                    ""type"": ""Button"",
                    ""id"": ""fc04b280-e791-46b5-8171-2d71e5326524"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""next kit"",
                    ""type"": ""Button"",
                    ""id"": ""b0b5af6f-17bb-42f7-a692-5bf96f8990ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""start"",
                    ""type"": ""Button"",
                    ""id"": ""1635de8c-b097-47a6-a76d-6654c7911ec7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b339390f-605d-43b7-9a5c-ae77e46a91d4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""previous kit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0fdb97f-3fee-418b-9369-faaf6878ec59"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""previous kit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ab7283e-656f-47e7-bf41-cd34ea641086"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""next kit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64aaf454-ffad-49c0-9e1a-0657a6ae03fc"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""next kit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cd775b68-140c-4807-8ed2-940ad8ee965c"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard + Mouse"",
                    ""action"": ""start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e9bdf08-cb1b-498c-88c2-7504e9463ab1"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard + Mouse"",
            ""bindingGroup"": ""Keyboard + Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // gameplay
        m_gameplay = asset.FindActionMap("gameplay", throwIfNotFound: true);
        m_gameplay_previouskit = m_gameplay.FindAction("previous kit", throwIfNotFound: true);
        m_gameplay_nextkit = m_gameplay.FindAction("next kit", throwIfNotFound: true);
        m_gameplay_start = m_gameplay.FindAction("start", throwIfNotFound: true);
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

    // gameplay
    private readonly InputActionMap m_gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_gameplay_previouskit;
    private readonly InputAction m_gameplay_nextkit;
    private readonly InputAction m_gameplay_start;
    public struct GameplayActions
    {
        private @ChoosePlayerInputs m_Wrapper;
        public GameplayActions(@ChoosePlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @previouskit => m_Wrapper.m_gameplay_previouskit;
        public InputAction @nextkit => m_Wrapper.m_gameplay_nextkit;
        public InputAction @start => m_Wrapper.m_gameplay_start;
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @previouskit.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviouskit;
                @previouskit.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviouskit;
                @previouskit.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPreviouskit;
                @nextkit.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextkit;
                @nextkit.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextkit;
                @nextkit.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnNextkit;
                @start.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @start.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @start.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @previouskit.started += instance.OnPreviouskit;
                @previouskit.performed += instance.OnPreviouskit;
                @previouskit.canceled += instance.OnPreviouskit;
                @nextkit.started += instance.OnNextkit;
                @nextkit.performed += instance.OnNextkit;
                @nextkit.canceled += instance.OnNextkit;
                @start.started += instance.OnStart;
                @start.performed += instance.OnStart;
                @start.canceled += instance.OnStart;
            }
        }
    }
    public GameplayActions @gameplay => new GameplayActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard + Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnPreviouskit(InputAction.CallbackContext context);
        void OnNextkit(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
    }
}
