import { View, Text, Pressable, StyleSheet, Dimensions, Image } from "react-native";
import { useNavigation } from "expo-router";

import { COLORS } from "../constants/theme";
import { apiUrl } from "../constants/config";

export function eraseCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999;';
}

export default function LogOut() {
    const navigation = useNavigation();
    const { width } = Dimensions.get('window');
    const isMobile = width < 600;

    async function logout() {
        const res = await fetch(`${apiUrl}Auth/Logout`, {
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
            <Pressable style={styles.pre} onPress={logout}>
                <Image source={require('../assets/logout.png')} style={styles.logout} />
            </Pressable>
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        position: 'absolute',
        top: 16,
        right: 16,
        zIndex: 10,
    },
    pressable: {
        backgroundColor: COLORS.white,
        paddingHorizontal: 20,
        justifyContent: 'center',
    },
    text: {
        color: COLORS.black,
        fontWeight: 'bold',
    },
    logout: {
        width: 40,
        height: 40,
    }
});