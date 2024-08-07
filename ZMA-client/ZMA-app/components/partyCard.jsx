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
            <Pressable onPress={onDelete} style={styles.deleteButton}>
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
      backgroundColor: COLORS.greyish,
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
      color: COLORS.white,
    },
    details: {
      fontSize: 16,
      color: COLORS.ashAndCreme,
      marginVertical: 4,
    },
    category: {
      fontSize: 16,
      color: COLORS.ashAndCreme,
      marginVertical: 4,
    },
    date: {
      fontSize: 14,
      color: COLORS.white,
      marginVertical: 4,
    },
    deleteButton: {
      backgroundColor: COLORS.accent,
      padding: 10,
      justifyContent: 'center',
      alignItems: 'center',
    },
    deleteButtonText: {
      color: '#fff',
      fontWeight: 'bold',
      fontSize: 'medium',
    },
    qrGenerateButton: {
      backgroundColor: COLORS.ashAndCreme,
      padding: 10,
      justifyContent: 'center',
      alignItems: 'center',
    },
    qrGenerateButtonText: {
      color: '#fff',
      fontWeight: 'bold',
    },
  });