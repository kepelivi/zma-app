import React from 'react';
import { View } from 'react-native';
import Svg, { Circle, Path, Line } from 'react-native-svg';

export default function LoginEye({ showPassword }) {
    return (
        <View>
            {showPassword ? (
                <Svg width="24" height="24" viewBox="0 0 64 64">
                    <Circle cx="32" cy="32" r="10" fill="black" />
                    <Path
                        d="M32,12 C16,12 4,32 4,32 C4,32 16,52 32,52 C48,52 60,32 60,32 C60,32 48,12 32,12 Z"
                        fill="none"
                        stroke="black"
                        strokeWidth="2"
                    />
                </Svg>
            ) : (
                <Svg width="24" height="24" viewBox="0 0 64 64">
                    <Circle cx="32" cy="32" r="10" fill="black" />
                    <Path
                        d="M32,12 C16,12 4,32 4,32 C4,32 16,52 32,52 C48,52 60,32 60,32 C60,32 48,12 32,12 Z"
                        fill="none"
                        stroke="black"
                        strokeWidth="2"
                    />
                    <Line x1="12" y1="12" x2="52" y2="52" stroke="red" strokeWidth="2" />
                </Svg>
            )}
        </View>
    );
}