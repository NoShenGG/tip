using System;
using System.Collections.Generic;
using Godot;

namespace Tip.Scripts.TimeMechanics; 

public class ObjectHistory {
    private const double MaxSecondsStored = 10.0;
    private double _currentTimeStored = 0.0;
    private List<PositionKeyframe> _positionHistory = new List<PositionKeyframe>();

    public void AddPositionKeyframe(Vector3 currentPosition, double delta) {
        _currentTimeStored += delta;
        
        // If necessary, lerp between oldest two positions to maintain max time
        if (_currentTimeStored > MaxSecondsStored) {
            PositionKeyframe first = _positionHistory[0];
            PositionKeyframe last = _positionHistory[1];
            
            double timeRemoved = _currentTimeStored - MaxSecondsStored;
            
            // Check to make sure bounding keyframes are sufficient to allocate timeRemoved
            // If not, iterate until accurate
            double boundsDelta = last.delta;
            int currentLastMarker = 1;
            while (boundsDelta < timeRemoved) {
                first = last;
                currentLastMarker++;
                last = _positionHistory[currentLastMarker];
                boundsDelta += last.delta;
            }

            float lerpWeightApproximation = (float) (boundsDelta - timeRemoved);
            Vector3 lerpPosition = last.position.Lerp(first.position, lerpWeightApproximation);

            // Remove extra keyframes and add interpolated keyframe
            for (int i = 0; i < currentLastMarker; i++) {
                _positionHistory.RemoveAt(0);
            }
            _positionHistory.Insert(0, new PositionKeyframe(lerpPosition, 0.0));

            _currentTimeStored = MaxSecondsStored;
        }
        
        _positionHistory.Add(new PositionKeyframe(currentPosition, delta));
    }

    public PositionKeyframe RemovePositionKeyframe() {
        int positionCount = _positionHistory.Count;
        if (positionCount != 0) {
            PositionKeyframe removedKeyframe = _positionHistory[positionCount - 1];
            _currentTimeStored -= removedKeyframe.delta;
            _positionHistory.RemoveAt(positionCount - 1);
            return removedKeyframe;
        }

        return null;
    }

    public PositionKeyframe GetPositionKeyframe(int index) {
        if (index > _positionHistory.Count - 1 || index < 0) {
            return null;
        }
        return _positionHistory[index];
    }

    public int Count() {
        return _positionHistory.Count;
    }

    public void Clear() {
        _positionHistory.Clear();
        _currentTimeStored = 0.0;
    }
}