using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class MidiReader : MonoBehaviour
    {
        [SerializeField] private EngineButtonPressEmitter _engineButtonPressEmitter;

        #region Callback implementation

        void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            // Debug.Log($"MIDI Device {change} ({device.description.product})");

            DisconnectAllDevices();
            ConnectAllDevices();
        }

        void OnWillNoteOn(Minis.MidiNoteControl note, float velocity)
        {
            EngineButton engineButton = new EngineButton(note.noteNumber);

            _engineButtonPressEmitter.OnPress?.Invoke(engineButton);

            // Debug.Log($"Clicked button {engineButton.ButtonIndex}.\n" +
            //           $"{engineButton.InSectionIndex} in {DashboardLayout.Sections[engineButton.SectionIndex]}");
        }

        void OnWillNoteOff(Minis.MidiNoteControl note)
        {
            EngineButton engineButton = new EngineButton(note.noteNumber);

            _engineButtonPressEmitter.OnRelease?.Invoke(engineButton);

            // Debug.Log($"Ch.{note.channel,-2} " +
            //           $"{note.shortDisplayName,3} ({note.noteNumber:000}) " +
            //           "Note Off");
        }

        void OnWillAftertouch(Minis.MidiNoteControl note, float pressure)
            => Debug.Log($"Ch.{note.channel,-2} " +
                         $"{note.shortDisplayName,3} ({note.noteNumber:000}) " +
                         $"Pressure {pressure * 100,3:0}%");

        void OnWillControlChange(Minis.MidiValueControl cc, float value)
        {
            // Debug.Log($"Ch.{cc.channel,-2} " +
            //           $"CC ({cc.controlNumber:000}) {value * 100,3:0}%");
        }

        #endregion

        #region Device detection

        private readonly List<Minis.MidiDevice> _devices = new();

        void ConnectAllDevices()
        {
            foreach (var device in InputSystem.devices)
            {
                var midiDevice = device as Minis.MidiDevice;
                if (midiDevice == null) continue;

                midiDevice.onWillNoteOn += OnWillNoteOn;
                midiDevice.onWillNoteOff += OnWillNoteOff;
                // midiDevice.onWillAftertouch += OnWillAftertouch;
                midiDevice.onWillControlChange += OnWillControlChange;

                _devices.Add(midiDevice);
            }
        }

        void DisconnectAllDevices()
        {
            foreach (var midiDevice in _devices)
            {
                midiDevice.onWillNoteOn -= OnWillNoteOn;
                midiDevice.onWillNoteOff -= OnWillNoteOff;
                // midiDevice.onWillAftertouch -= OnWillAftertouch;
                midiDevice.onWillControlChange -= OnWillControlChange;
            }

            _devices.Clear();
        }

        #endregion

        void Start()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
            ConnectAllDevices();
        }


        void OnDestroy()
        {
            DisconnectAllDevices();
            InputSystem.onDeviceChange -= OnDeviceChange;
        }
    }
}