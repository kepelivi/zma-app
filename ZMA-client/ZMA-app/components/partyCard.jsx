import { Text, View, Pressable, StyleSheet } from "react-native";
import { useState } from "react";
import { url } from "../constants/config";
import { COLORS } from "../constants/theme";
import QRCodeGenerator from "./QRCode";

export default function PartyCard({ party, onDelete, onPress }) {
  const [value, setValue] = useState(null);

    return (
        <View style={styles.card}>
            <Pressable onPress={onPress} style={styles.cardContent}>
                <View>
                    <Text style={styles.title}>{party.name}</Text>
                    <Text style={styles.details}>{party.details}</Text>
                    <Text style={styles.category}>{party.category}</Text>
                    <Text style={styles.date}>{party.date.split("T").shift()}</Text>
                </View>
            </Pressable>
            <Pressable onPress={onDelete} style={({ pressed }) => [
                styles.deleteButton,
                { backgroundColor: pressed ? '#7c1d82' : '#9c27b0' }
            ]}>
                <Text style={styles.deleteButtonText}>Törlés</Text>
            </Pressable>
            <Pressable style={styles.qrGenerateButton} onPress={() => setValue(`${url}requestSong?partyId=${party.id}`)}>
              <Text style={styles.qrGenerateButtonText}>QR kód létrehozása</Text>
            </Pressable>
            {value && (<QRCodeGenerator value={value} />)}
        </View>
    )
}

const styles = StyleSheet.create({
    card: {
      borderRadius: 10,
      backgroundColor: '#fff',
      shadowColor: '#000',
      shadowOffset: { width: 0, height: 2 },
      shadowOpacity: 0.3,
      shadowRadius: 4,
      elevation: 5,
      marginVertical: 10,
      marginHorizontal: 16,
      overflow: 'hidden',
    },
    cardContent: {
      padding: 16,
    },
    title: {
      fontSize: 20,
      fontWeight: 'bold',
      color: '#3f0257',
    },
    details: {
      fontSize: 16,
      color: '#3f0257',
      marginVertical: 4,
    },
    category: {
      fontSize: 16,
      color: '#9c27b0',
      marginVertical: 4,
    },
    date: {
      fontSize: 14,
      color: '#3f0257',
      marginVertical: 4,
    },
    deleteButton: {
      backgroundColor: '#9c27b0',
      padding: 10,
      justifyContent: 'center',
      alignItems: 'center',
    },
    deleteButtonText: {
      color: '#fff',
      fontWeight: 'bold',
    },
    qrGenerateButton: {
      backgroundColor: COLORS.deepPurple,
      padding: 10,
      justifyContent: 'center',
      alignItems: 'center',
    },
    qrGenerateButtonText: {
      color: '#fff',
      fontWeight: 'bold',
    },
  });