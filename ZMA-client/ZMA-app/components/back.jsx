import { View, Text, Pressable, StyleSheet } from "react-native";
import { useNavigation } from "expo-router";

import { COLORS } from "../constants/theme";

export default function GoBack() {
    const navigation = useNavigation();

    return (
        <View style={styles.container}>
            <Pressable style={styles.backButton} onPress={() => navigation.canGoBack() && navigation.goBack()}>
                <Text style={styles.text}>⮜</Text>
            </Pressable>
        </View>
    )
}

const styles = StyleSheet.create({
    container: {
        position: 'absolute',
        top: 0,
        left: 0,
        padding: 16,
    },
    backButton: {
        backgroundColor: COLORS.deepPurple,
        borderWidth: 1,
        borderRadius: 5,
        borderColor: COLORS.greyish,
        padding: 10,
    },
    text: {
        color: COLORS.white,
        fontWeight: 'bold',
        fontSize: 'medium',
    }
})