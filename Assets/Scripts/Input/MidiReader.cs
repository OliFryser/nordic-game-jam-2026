using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class MidiReader : MonoBehaviour
    {
        public ModuleManager ModuleManager { get; set; }

        #region Callback implementation

        void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            Debug.Log($"MIDI Device {change} ({device.description.product})");

            DisconnectAllDevices();
            ConnectAllDevices();
        }

        void OnWillNoteOn(Minis.MidiNoteControl note, float velocity)
        {
            // Debug.Log($"Ch.{note.channel,-2} " +
            //              $"{note.shortDisplayName,3} ({note.noteNumber:000}) " +
            //              $"Note On  {velocity * 100,3:0}%");

            int sectionIndex = Math.Min(note.noteNumber / DashboardLayout.SectionLength, DashboardLayout.Sections.Length - 1);
            int buttonIndex = note.noteNumber % DashboardLayout.SectionLength;

            // Make overflow keys belong to last section
            if (note.noteNumber >= DashboardLayout.TotalKeys - 1)
            {
                buttonIndex = DashboardLayout.SectionLength + buttonIndex;
            }

            ModuleManager.Tick(sectionIndex, buttonIndex);
            
            Debug.Log($"Clicked button {buttonIndex} in {DashboardLayout.Sections[sectionIndex]}");
            
        }

        void OnWillNoteOff(Minis.MidiNoteControl note)
        {
            // Debug.Log($"Ch.{note.channel,-2} " +
            //           $"{note.shortDisplayName,3} ({note.noteNumber:000}) " +
            //           "Note Off");
        }

        void OnWillAftertouch(Minis.MidiNoteControl note, float pressure)
            => Debug.Log($"Ch.{note.channel,-2} " +
                         $"{note.shortDisplayName,3} ({note.noteNumber:000}) " +
                         $"Pressure {pressure * 100,3:0}%");

        void OnWillControlChange(Minis.MidiValueControl cc, float value)
            => Debug.Log($"Ch.{cc.channel,-2} " +
                         $"CC ({cc.controlNumber:000}) {value * 100,3:0}%");

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
            ModuleManager = FindAnyObjectByType<ModuleManager>();
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