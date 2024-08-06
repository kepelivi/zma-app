import React from "react";
import { View, StyleSheet } from "react-native";
import QRCode from "react-native-qrcode-svg";

export default function QRCodeGenerator({ value, size = 200 }) {
    return (
        <View style={styles.container}>
            <QRCode value={value} size={size} />
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        justifyContent: 'center',
        alignItems: 'center',
        padding: 10,
    }
})