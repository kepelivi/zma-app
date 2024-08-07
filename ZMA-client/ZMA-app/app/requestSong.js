import { useState } from "react";
import { View, Text, TextInput, Pressable, StyleSheet, SafeAreaView } from "react-native";
import { useLocalSearchParams } from 'expo-router';
import { apiUrl } from "../constants/config";
import Logo from "../components/logo";
import { COLORS } from "../constants/theme";
import { notification } from "antd";

notification.config({
    duration: 2,
    closeIcon: null
})

export default function requestSong() {
    const params = useLocalSearchParams();

    const [title, setTitle] = useState("");

    async function handleRequest() {
        try {
            const response = await fetch(`${apiUrl}Song/RequestSong?title=${encodeURIComponent(title)}&partyId=${encodeURIComponent(params.partyId)}`, {
                method: 'POST',
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' }
            });

            if (!response.ok) {
                throw new Error("Request failed with status " + response.status);
            }

            const data = await response.json();
            notification.success({ message: "Zene kérés sikeres." });
            console.log("Request successful:", data);
        } catch (error) {
            console.error("Error during fetch:", error);
            notification.error({ message: "Zene kérése sikertelen." });
        }
    }

    return (
        <SafeAreaView style={styles.safeArea}>
            <Logo />
            <View style={styles.container}>
                <Text style={styles.mainText}>Kérj egy zenét!</Text>
                <View style={styles.inputContainer}>
                    <TextInput
                        style={styles.input}
                        placeholder="Cím és előadó"
                        value={title}
                        onChangeText={setTitle}
                        autoCorrect={false}
                        autoCapitalize="none"
                        placeholderTextColor={COLORS.greyish}
                    />
                    <Pressable style={styles.button} onPress={handleRequest}>
                        <Text style={styles.buttonText}>Küldés</Text>
                    </Pressable>
                </View>
            </View>
        </SafeAreaView>
    )
}

const styles = StyleSheet.create({
    safeArea: {
        flex: 1,
    },
    container: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
    },
    mainText: {
        fontSize: 24,
        fontWeight: 'bold',
        color: COLORS.accent,
        marginBottom: 20,
    },
    button: {
        justifyContent: 'center',
        alignItems: 'center',
        paddingVertical: 12,
        paddingHorizontal: 24,
        borderRadius: 8,
        backgroundColor: COLORS.ashAndCreme,
        marginTop: 16,
    },
    buttonText: {
        color: COLORS.white,
        fontSize: 16,
    },
    input: {
        width: '100%',
        paddingVertical: 8,
        paddingHorizontal: 16,
        borderWidth: 1,
        borderColor: COLORS.greyish,
        borderRadius: 8,
        marginBottom: 16,
        backgroundColor: COLORS.white,
        width: '80%',
    },
    inputContainer: {
        width: '80%',
        alignItems: 'center',
    },
});