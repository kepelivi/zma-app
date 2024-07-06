import { View, Text, Pressable, StyleSheet } from "react-native";
import { useNavigation } from "expo-router";

import { COLORS } from "../constants/theme";
import { apiUrl } from "../constants/config";

export function eraseCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999;';
}

export default function LogOut() {
    const navigation = useNavigation();

    async function logout() {
        const res = await fetch(`${apiUrl}/Auth/Logout`, {
            method: 'POST',
            credentials: 'include',
            headers: { 'Content-Type': 'application/json' },
        })
        if (!res.ok) {
            throw new Error("Logout failed.");
        }
        eraseCookie("Host");
        navigation.navigate('index');
    }

    return (
        <View style={styles.container}>
            <Pressable style={styles.pressable} onPress={logout}>
                <Text style={styles.text}>Kijelentkezés</Text>
            </Pressable>
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        justifyContent: 'flex-start',
        alignItems: 'flex-end',
        padding: 16,
    },
    pressable: {
        backgroundColor: COLORS.deepPurple,
        borderWidth: 1,
        borderRadius: 5,
        borderColor: COLORS.deepPurple,
        padding: 10,
        position: 'absolute',
        top: 16,
        right: 16,
    },
    text: {
        color: COLORS.white,
        fontWeight: 'bold',
    },
});