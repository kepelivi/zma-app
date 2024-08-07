import { Text, View, Pressable, StyleSheet } from "react-native";

import { COLORS } from "../constants/theme";

export default function SongCard({ song, onAccept, onDeny }) {
    return (
        <View style={styles.card}>
            <View style={styles.cardContent}>
                <Text style={styles.title}>{song.title}</Text>
                <Text style={styles.time}>{song.requestTime.split("T").shift()} - {song.requestTime.split("T").pop().split('.').shift()}</Text>
            </View>
            <View style={styles.buttonContainer}>
                <Pressable onPress={onAccept}>
                    <Text style={[styles.button, styles.accept]}>Elfogad</Text>
                </Pressable>
                <Pressable onPress={onDeny}>
                    <Text style={[styles.button, styles.deny]}>Elutasít</Text>
                </Pressable>
            </View>
        </View>
    )
}

const styles = StyleSheet.create({
    card: {
        backgroundColor: COLORS.accent,
        padding: 16,
        borderRadius: 8,
        marginBottom: 16,
        elevation: 2,
    },
    cardContent: {
        marginBottom: 16,
    },
    title: {
        fontSize: 18,
        fontWeight: 'bold',
        color: COLORS.white,
        marginBottom: 8,
    },
    time: {
        color: COLORS.ashAndCreme,
    },
    buttonContainer: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
    button: {
        paddingVertical: 8,
        paddingHorizontal: 16,
        borderRadius: 4,
        textAlign: 'center',
        fontWeight: 'bold',
        fontSize: 16,
        alignSelf: 'flex-start',
    },
    accept: {
        backgroundColor: COLORS.ashAndCreme,
        color: '#FFFFFF',
    },
    deny: {
        backgroundColor: COLORS.greyish,
        color: '#FFFFFF',
    },
});